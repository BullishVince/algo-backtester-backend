using System.Globalization;
using AlgoBacktesterBackend.Domain.Models;
using AlgoBacktesterBackend.Extensions;

namespace AlgoBacktesterBackend.Domain.Repository;
public interface IAssetPairRepository
{
    public AssetPair GetHistoricalAssetPairData(string ticker, Timeframe timeframe, DateTime? startDate, DateTime? endDate);
    public Task<AssetPair> GetHistoricalAssetPairDataFromFile(string ticker, Timeframe timeframe, string fileName);
}
public class AssetPairRepository : IAssetPairRepository
{
    public AssetPair GetHistoricalAssetPairData(string ticker, Timeframe timeframe, DateTime? startDate, DateTime? endDate)
    {
        return new AssetPair(ticker, timeframe);
    }
    public async Task<AssetPair> GetHistoricalAssetPairDataFromFile(string ticker, Timeframe timeframe, string fileName)
    {
        var assetPair = new AssetPair(ticker, timeframe);

        int counter = 0;
        foreach (string line in await File.ReadAllLinesAsync(fileName))
        {
            if (counter < ((int)timeframe))
            {
                counter++;
            }
            else
            {
                var data = line.Split(';', 6, StringSplitOptions.RemoveEmptyEntries);

                //Fix the formatting since the DateTime field in the csv's from HistData.com is invalid
                data[0] = data[0].MultiInsert("-", 3, 5).Replace(" ", "T").MultiInsert(":", 12, 14);

                try
                {
                    var dataPoint = new DataPoint(
                        DateTime.Parse(data[0], CultureInfo.InvariantCulture),
                        decimal.Parse(data[1], CultureInfo.InvariantCulture),
                        decimal.Parse(data[2], CultureInfo.InvariantCulture),
                        decimal.Parse(data[3], CultureInfo.InvariantCulture),
                        decimal.Parse(data[4], CultureInfo.InvariantCulture),
                        decimal.Parse(data[5], CultureInfo.InvariantCulture));
                    assetPair.AddDataPoint(dataPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                counter = 0;
            }
        }
        return assetPair;
    }
}