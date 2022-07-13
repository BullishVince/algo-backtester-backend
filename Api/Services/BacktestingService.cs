using AlgoBacktesterBackend.Api.Config.Messages;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Domain.Models;
using AlgoBacktesterBackend.Domain.Repository;
using Skender.Stock.Indicators;

namespace AlgoBacktesterBackend.Api.Services;
public interface IBacktestingService {
    public Task<IResponseMessage<BacktestingStatistics>> Backtest(BacktestingRequest backtestingRequest);
}
public class BacktestingService: IBacktestingService {
    private const string testFile = "Database/Files/EURUSD/EURUSD_M1_202201.csv";
    private DateTime StartDate {get; set;}
    private DateTime EndDate {get; set;}
    private decimal InitialCapital {get; set;}
    private IAssetPairRepository _assetPairRepository {get; set;}
    public BacktestingService(IAssetPairRepository assetPairRepository) {
        _assetPairRepository = assetPairRepository;
    }
    public async Task<IResponseMessage<BacktestingStatistics>> Backtest(BacktestingRequest backtestingRequest) {
        if (backtestingRequest.BacktestingPairs.Count() == 0) {
            return new ResponseMessage<BacktestingStatistics>(
                Status.Error, 
                new string[]{"Need at least one backtesting pair"}, 
                new BacktestingStatistics(backtestingRequest.StartDate.HasValue ? backtestingRequest.StartDate.Value : DateTime.MinValue));
        }

        // var assetPair = backtestingRequest.BacktestingPairs.Select(
        //     p => _assetPairRepository.GetHistoricalAssetPairData(
        //         p, 
        //         backtestingRequest.StartDate,
        //         backtestingRequest.EndDate)
        // );
        StartDate = backtestingRequest.StartDate.HasValue ? backtestingRequest.StartDate.Value : DateTime.MinValue;
        EndDate = backtestingRequest.EndDate.HasValue ? backtestingRequest.EndDate.Value : DateTime.MaxValue;
        InitialCapital = backtestingRequest.InitialCapital;
        var timeframe = new Timeframe(backtestingRequest.Strategy.TimeframeFilter.ExecutingTimeframe);
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile("EURUSD", timeframe, testFile);

        var result = await RunBacktestOnPair(timeframe, assetPair);
        return new ResponseMessage<BacktestingStatistics>(Status.Success,null, result);

    }

    private async Task<BacktestingStatistics> RunBacktestOnPair(Timeframe timeframe, AssetPair assetPair) {
        var currentCapital = InitialCapital;
        var stats = new BacktestingStatistics(StartDate);
        var trades = new List<Trade>();

        foreach(DataPoint dataPoint in assetPair.DataPoints) {
            //Refresh trades on each datapoint in case some trades need to be closed or adjusted
            trades = await RefreshTrades(trades, dataPoint, currentCapital);

            //TODO: Replace hardcoded strategy with a dynamic solution. Code below is temporary used for testing
            if (dataPoint.Close > dataPoint.Open && trades.FindAll(t => !t.CloseDate.HasValue).Count == 0) {
                trades.Add(Trade.OpenTrade(
                    dataPoint.Date,
                    assetPair.TickerName,
                    "LONG",
                    (decimal)0.1,
                    dataPoint.Open - (decimal)0.0001,
                    dataPoint.Open + (decimal)0.0002,
                    dataPoint.Open));
            }

        }
        stats = await CompleteBacktestingSession(stats, trades, assetPair.DataPoints.Last(), currentCapital);
        return stats;
    }

    private async Task<List<Trade>> RefreshTrades(List<Trade> trades, DataPoint dataPoint, decimal currentCapital) {
        foreach(Trade trade in trades.FindAll(t => !t.CloseDate.HasValue)) {
                if (trade.Action == "LONG") {
                    if (dataPoint.IsDataPointBelowPrice(trade.StopLoss.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.StopLoss.GetValueOrDefault(), currentCapital);
                    } else if (dataPoint.IsDataPointAbovePrice(trade.TakeProfit.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.TakeProfit.GetValueOrDefault(), currentCapital);
                    }
                } else if (trade.Action == "SHORT") {
                    if (dataPoint.IsDataPointBelowPrice(trade.TakeProfit.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.TakeProfit.GetValueOrDefault(), currentCapital);
                    } else if (dataPoint.IsDataPointAbovePrice(trade.StopLoss.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.StopLoss.GetValueOrDefault(), currentCapital);
                    }
                }
        }
        return trades;
    }

    private async Task<BacktestingStatistics> CompleteBacktestingSession(BacktestingStatistics stats, List<Trade> trades, DataPoint lastDataPoint, decimal currentCapital) {
        foreach (Trade trade in trades) {
            if (!trade.CloseDate.HasValue) {
                trade.CloseTrade(lastDataPoint.Date, lastDataPoint.Close, currentCapital);
            }
        }

        stats.BestTrade = trades.Max(t => t.NetProfit);
        stats.WorstTrade = trades.Min(t => t.NetProfit);
        stats.NetProfit = currentCapital - InitialCapital;
        stats.PercentageGain = (currentCapital/InitialCapital - 1)*100;
        stats.EndDate = trades.Max(t => t.CloseDate.Value);
        stats.NumberOfTrades = trades.Count;
        stats.Profitability = trades.Where(t => t.NetProfit > 0).Count() / trades.Count;
        stats.NumberOfDaysBacktested = ((int)trades.Max(t => t.CloseDate.Value).Subtract(trades.Min(t => t.OpenDate)).TotalDays);
        return stats;
    }
}