using AlgoBacktesterBackend.Domain.Models;

namespace AlgoBacktesterBackend.Domain.Repository;
public interface IAssetPairRepository {
    public AssetPair GetHistoricalAssetPairData(string ticker, DateTime? startDate, DateTime? endDate);
    public Task<AssetPair> GetHistoricalAssetPairDataFromFile(string ticker, string fileName);
}
public class AssetPairRepository : IAssetPairRepository {
    public AssetPair GetHistoricalAssetPairData(string ticker, DateTime? startDate, DateTime? endDate) {
        return new AssetPair(ticker);
    }

    public async Task<AssetPair> GetHistoricalAssetPairDataFromFile(string ticker, string fileName) {
        var assetPair = new AssetPair(ticker);
        foreach (string line in await File.ReadAllLinesAsync(fileName)) {
            var data = line.Split(';', 6, StringSplitOptions.RemoveEmptyEntries);
            try {
                var dataPoint = new DataPoint(
                    DateTime.Parse(data[0]), 
                    decimal.Parse(data[1]), 
                    decimal.Parse(data[2]), 
                    decimal.Parse(data[3]), 
                    decimal.Parse(data[4]), 
                    decimal.Parse(data[5]));
                assetPair.AddDataPoint(dataPoint);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
        return assetPair;
    }
}