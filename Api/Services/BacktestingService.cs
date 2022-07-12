using AlgoBacktesterBackend.Api.Config.Messages;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Domain.Models;
using AlgoBacktesterBackend.Domain.Repository;

namespace AlgoBacktesterBackend.Api.Services;
public interface IBacktestingService {
    public Task<IResponseMessage<BacktestingResult>> RunBacktest(BacktestingRequest backtestingRequest);
}
public class BacktestingService: IBacktestingService {
    private const string testFile = "Database/Files/EURUSD/EURUSD_M1_202201.csv";
    private IAssetPairRepository _assetPairRepository {get; set;}
    public BacktestingService(IAssetPairRepository assetPairRepository) {
        _assetPairRepository = assetPairRepository;
    }
    public async Task<IResponseMessage<BacktestingResult>> RunBacktest(BacktestingRequest backtestingRequest) {
        if (backtestingRequest.BacktestingPairs.Count() == 0) {
            return new ResponseMessage<BacktestingResult>(
                Status.Error, 
                new string[]{"Need at least one backtesting pair"}, 
                new BacktestingResult());
        }

        // var assetPair = backtestingRequest.BacktestingPairs.Select(
        //     p => _assetPairRepository.GetHistoricalAssetPairData(
        //         p, 
        //         backtestingRequest.StartDate,
        //         backtestingRequest.EndDate)
        // );
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile("EURUSD", Timeframe.M1, testFile);

        return null;

    }
}