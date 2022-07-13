using AlgoBacktesterBackend.Api.Config.Messages;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Domain.Models;
using AlgoBacktesterBackend.Domain.Repository;

namespace AlgoBacktesterBackend.Api.Services;
public interface IAssetPairService {
    public Task<IResponseMessage<AssetPair>> GetAssetPairDataFromFile(string fileName, string timeframe);
}
public class AssetPairService: IAssetPairService {
    private const string testFile = "Database/Files/EURUSD/EURUSD_M1_202201.csv";
    private IAssetPairRepository _assetPairRepository {get; set;}
    public AssetPairService(IAssetPairRepository assetPairRepository) {
        _assetPairRepository = assetPairRepository;
    }
    public async Task<IResponseMessage<AssetPair>> GetAssetPairDataFromFile(string fileName, string timeframe) {
        var tf = new Timeframe(timeframe);
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile(string.Empty, tf, fileName);
        return new ResponseMessage<AssetPair>(Status.Success, null, assetPair);
    }
}