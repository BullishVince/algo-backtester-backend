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

        var dataPoints = new List<DataPoint>();
        foreach (string line in await File.ReadAllLinesAsync(fileName))
        {
            var data = line.Split(';', 6, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                //Fix the formatting since the DateTime field in the csv's from HistData.com is invalid
                data[0] = data[0].MultiInsert("-", 3, 5).Replace(" ", "T").MultiInsert(":", 12, 14);

                var dataPoint = new DataPoint(
                    DateTime.Parse(data[0], CultureInfo.InvariantCulture),
                    decimal.Parse(data[1], CultureInfo.InvariantCulture),
                    decimal.Parse(data[2], CultureInfo.InvariantCulture),
                    decimal.Parse(data[3], CultureInfo.InvariantCulture),
                    decimal.Parse(data[4], CultureInfo.InvariantCulture),
                    decimal.Parse(data[5], CultureInfo.InvariantCulture));
                dataPoints.Add(dataPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        switch (timeframe.Type) {
            case TimeframeType.Minutes:
                foreach (DataPoint dataPoint in dataPoints) {
                    if (assetPair.DataPoints.Count == 0) {
                        assetPair.AddDataPoint(dataPoint);
                    } else {
                        if (dataPoint.Date.Subtract(assetPair.DataPoints[assetPair.DataPoints.Count - 1].Date).TotalMinutes >= timeframe.Interval) {
                            assetPair.AddDataPoint(dataPoint);
                        }
                    }
                }
                break;
            case TimeframeType.Hours:
                foreach (DataPoint dataPoint in dataPoints) {
                    if (assetPair.DataPoints.Count == 0) {
                        assetPair.AddDataPoint(dataPoint);
                    } else {
                        if (dataPoint.Date.Subtract(assetPair.DataPoints[assetPair.DataPoints.Count - 1].Date).TotalHours >= timeframe.Interval) {
                            assetPair.AddDataPoint(dataPoint);
                        }
                    }
                }
                break;
            case TimeframeType.Days:
            case TimeframeType.Weeks:
                foreach (DataPoint dataPoint in dataPoints) {
                    if (assetPair.DataPoints.Count == 0) {
                        assetPair.AddDataPoint(dataPoint);
                    } else {
                        if (dataPoint.Date.Subtract(assetPair.DataPoints[assetPair.DataPoints.Count - 1].Date).TotalDays >= timeframe.Interval) {
                            assetPair.AddDataPoint(dataPoint);
                        }
                    }
                }
                break;
            default:
            break;
        }

        return assetPair;
    }
}