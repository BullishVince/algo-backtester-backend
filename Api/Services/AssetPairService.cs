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
        var root = Directory.GetParent(Environment.CurrentDirectory).ToString();
        Timeframe tf;
        if (Enum.TryParse(timeframe, true, out tf)) {
            var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile(string.Empty, tf, Path.Combine(root, fileName));
            return new ResponseMessage<AssetPair>(Status.Success, null, assetPair);
        } else {
            return new ResponseMessage<AssetPair>(Status.Error, new string[]{"Could not parse timeframe"}, null);
        }
    }
}