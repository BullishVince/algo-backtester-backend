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
        var timeframe = new Timeframe(backtestingRequest.Strategy.TimeframeFilter.ExecutingTimeframe);
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile("EURUSD", timeframe, testFile);

        var result = await RunBacktestOnPair(timeframe, assetPair);
        return new ResponseMessage<BacktestingStatistics>(Status.Success,null, result);

    }

    private async Task<BacktestingStatistics> RunBacktestOnPair(Timeframe timeframe, AssetPair assetPair) {
        var stats = new BacktestingStatistics(StartDate);
        var trades = new List<Trade>();
        foreach(DataPoint dataPoint in assetPair.DataPoints) {
            //Refresh trades on each datapoint in case some trades need to be closed or adjusted
            trades = await RefreshTrades(trades, dataPoint);

            //TODO: Replace hardcoded strategy with a dynamic solution
            if (dataPoint.Close > dataPoint.Open && trades.Count == 0) {
                // trades.Add(Trade.OpenTrade());
            }

        }
        
        return stats;
    }

    private async Task<List<Trade>> RefreshTrades(List<Trade> trades, DataPoint dataPoint) {
        foreach(Trade trade in trades) {
                if (trade.Action == "LONG") {

                } else if (trade.Action == "SHORT") {

                } else {

                }
        }
        return trades;
    }
}