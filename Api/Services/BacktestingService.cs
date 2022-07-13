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
    private IAssetPairRepository _assetPairRepository {get; set;}
    public BacktestingService(IAssetPairRepository assetPairRepository) {
        _assetPairRepository = assetPairRepository;
    }
    public async Task<IResponseMessage<BacktestingStatistics>> Backtest(BacktestingRequest backtestingRequest) {
        if (backtestingRequest.BacktestingPairs.Count() == 0) {
            return new ResponseMessage<BacktestingStatistics>(
                Status.Error, 
                new string[]{"Need at least one backtesting pair"}, 
                new BacktestingStatistics());
        }

        // var assetPair = backtestingRequest.BacktestingPairs.Select(
        //     p => _assetPairRepository.GetHistoricalAssetPairData(
        //         p, 
        //         backtestingRequest.StartDate,
        //         backtestingRequest.EndDate)
        // );

        var timeframe = new Timeframe(backtestingRequest.TimeframeFilter.ExecutingTimeframe);
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile("EURUSD", timeframe, testFile);



        return null;

    }

    private BacktestingStatistics RunBacktest(Timeframe timeframe, AssetPair assetPair) {
        var stats = new BacktestingStatistics(){

        };

        foreach(DataPoint dataPoint in assetPair.DataPoints) {

        }
        
        return null;
    }
}