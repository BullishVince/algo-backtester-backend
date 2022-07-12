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
        Timeframe tf = MapStringToTimeframe(timeframe); //TODO: Handle errors here in case we receive incorrect input
        var assetPair = await _assetPairRepository.GetHistoricalAssetPairDataFromFile(string.Empty, tf, Path.Combine(root, fileName));
        return new ResponseMessage<AssetPair>(Status.Success, null, assetPair);
    }

    private Timeframe MapStringToTimeframe(string timeframe) {
        switch (timeframe) {
            case "M1":
                return new Timeframe(TimeframeType.Minutes, 1);
            case "M5":
                return new Timeframe(TimeframeType.Minutes, 5);
            case "M15":
                return new Timeframe(TimeframeType.Minutes, 15);
            case "M30":
                return new Timeframe(TimeframeType.Minutes, 30);
            case "H1":
                return new Timeframe(TimeframeType.Hours, 1);
            case "H2":
                return new Timeframe(TimeframeType.Hours, 2);
            case "H4":
                return new Timeframe(TimeframeType.Hours, 4);
            case "H8":
                return new Timeframe(TimeframeType.Hours, 8);
            case "H12":
                return new Timeframe(TimeframeType.Hours, 12);
            case "D1":
                return new Timeframe(TimeframeType.Days, 1);
            case "W1":
                return new Timeframe(TimeframeType.Weeks, 1);
            default:
                return new Timeframe(TimeframeType.Days, 1);
        }
    }
}