using AlgoBacktesterBackend.Api.Config.Messages;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Domain.Models;
using AlgoBacktesterBackend.Domain.Repository;
using Skender.Stock.Indicators;

namespace AlgoBacktesterBackend.Api.Services;
public interface IBacktestingService {
    public Task<IResponseMessage<List<BacktestingStatistics>>> Backtest(BacktestingRequest backtestingRequest);
}
public class BacktestingService: IBacktestingService {
    private const string testFile = "Database/Files/EURUSD/EURUSD_M1_202201.csv";
    private DateTime StartDate {get; set;}
    private DateTime EndDate {get; set;}
    private decimal InitialCapital {get; set;}
    private IAssetPairRepository _assetPairRepository {get;}
    private ApplicationSettings _applicationSettings {get;}
    public BacktestingService(IAssetPairRepository assetPairRepository, ApplicationSettings applicationSettings) {
        _applicationSettings = applicationSettings;
        _assetPairRepository = assetPairRepository;
    }
    public async Task<IResponseMessage<List<BacktestingStatistics>>> Backtest(BacktestingRequest backtestingRequest) {
        if (backtestingRequest.BacktestingPairs.Count() == 0) {
            return new ResponseMessage<List<BacktestingStatistics>>(
                Status.Error, 
                new string[]{"Need at least one backtesting pair"}, 
                new List<BacktestingStatistics>(){new BacktestingStatistics(backtestingRequest.StartDate.HasValue ? backtestingRequest.StartDate.Value : DateTime.MinValue, string.Empty)});
        }
        var backtestingResults = new List<BacktestingStatistics>();
        var timeframe = new Timeframe(backtestingRequest.Strategy.TimeframeFilter.ExecutingTimeframe);

        var backtestingPairs = new List<AssetPair>();
        foreach (string backtestingPair in backtestingRequest.BacktestingPairs) {
            backtestingPairs.Add(await _assetPairRepository.GetHistoricalAssetPairDataFromFile(
                backtestingPair, 
                timeframe, 
                $"Database/Files/{backtestingPair}/{backtestingPair}_M1_202201.csv"));
        }

        foreach (AssetPair assetPair in backtestingPairs) {
            StartDate = backtestingRequest.StartDate.HasValue ? backtestingRequest.StartDate.Value : DateTime.MinValue;
            EndDate = backtestingRequest.EndDate.HasValue ? backtestingRequest.EndDate.Value : DateTime.MaxValue;
            InitialCapital = backtestingRequest.InitialCapital;
            backtestingResults.Add(await RunBacktestOnPair(timeframe, assetPair));
        }

        return new ResponseMessage<List<BacktestingStatistics>>(Status.Success, null, backtestingResults);

    }

    private async Task<BacktestingStatistics> RunBacktestOnPair(Timeframe timeframe, AssetPair assetPair) {
        var currentCapital = InitialCapital;
        var stats = new BacktestingStatistics(StartDate, assetPair.TickerName);
        var trades = new List<Trade>();

        foreach(DataPoint dataPoint in assetPair.DataPoints) {
            //Refresh trades on each datapoint in case some trades need to be closed or adjusted
            trades = RefreshTrades(trades, dataPoint, currentCapital);

            //TODO: Replace hardcoded strategy with a dynamic solution. Code below is temporary used for testing
            if (dataPoint.Close > dataPoint.Open && trades.FindAll(t => !t.CloseDate.HasValue).Count == 0) {
                trades.Add(Trade.OpenTrade(
                    dataPoint.Date,
                    assetPair.TickerName,
                    "LONG",
                    (decimal)1.02,
                    dataPoint.Open - (decimal)0.0012,
                    dataPoint.Open + (decimal)0.0020,
                    dataPoint.Open));
            }

        }
        stats = CompleteBacktestingSession(stats, trades, assetPair.DataPoints.Last(), currentCapital);
        return stats;
    }

    private List<Trade> RefreshTrades(List<Trade> trades, DataPoint dataPoint, decimal currentCapital) {
        foreach(Trade trade in trades.FindAll(t => !t.CloseDate.HasValue)) {
                if (trade.Action == "LONG") {
                    if (dataPoint.IsDataPointBelowPrice(trade.StopLoss.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.StopLoss.GetValueOrDefault(), ref currentCapital);
                    } else if (dataPoint.IsDataPointAbovePrice(trade.TakeProfit.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.TakeProfit.GetValueOrDefault(), ref currentCapital);
                    }
                } else if (trade.Action == "SHORT") {
                    if (dataPoint.IsDataPointBelowPrice(trade.TakeProfit.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.TakeProfit.GetValueOrDefault(), ref currentCapital);
                    } else if (dataPoint.IsDataPointAbovePrice(trade.StopLoss.GetValueOrDefault())) {
                        trade.CloseTrade(dataPoint.Date, trade.StopLoss.GetValueOrDefault(), ref currentCapital);
                    }
                }
        }
        return trades;
    }

    private BacktestingStatistics CompleteBacktestingSession(BacktestingStatistics stats, List<Trade> trades, DataPoint lastDataPoint, decimal currentCapital) {
        foreach (Trade trade in trades) {
            if (!trade.CloseDate.HasValue) {
                trade.CloseTrade(lastDataPoint.Date, lastDataPoint.Close, ref currentCapital);
            }
        }

        stats.BestTrade = trades.Max(t => t.NetProfit);
        stats.WorstTrade = trades.Min(t => t.NetProfit);
        stats.NetProfit = currentCapital - InitialCapital;
        stats.PercentageGain = (currentCapital/InitialCapital - 1)*100;
        stats.EndDate = trades.Max(t => t.CloseDate.Value);
        stats.NumberOfTrades = trades.Count;
        stats.Profitability = trades.Where(t => t.NetProfit > 0).Count() / trades.Count;
        stats.AverageWin = trades.Where(t => t.NetProfit > 0).Average(t => t.NetProfit);
        stats.AverageLoss = trades.Where(t => t.NetProfit <= 0).Average(t => t.NetProfit);
        stats.AverageTradeLength = ((decimal)trades.Average(t => t.Duration));
        stats.NumberOfDaysBacktested = ((int)trades.Max(t => t.CloseDate.Value).Subtract(trades.Min(t => t.OpenDate)).TotalDays);
        
        stats.ProfitFactor = (stats.Profitability * stats.AverageWin) / ((1 - stats.Profitability)*stats.AverageLoss);
        //TODO: also add SharpeRatio, Standard Deviation, Expectancy, AHPR and GHPR
        return stats;
    }
}