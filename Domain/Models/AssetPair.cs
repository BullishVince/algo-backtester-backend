namespace AlgoBacktesterBackend.Domain.Models;
public class AssetPair {
    public AssetPair(string tickerName, Timeframe timeframe) {
        TickerName = tickerName;
        Timeframe = timeframe;
        DataPoints = new List<DataPoint>();
    }
    public string TickerName { get; } //ex. EURUSD, AAPL, BTCUSDT, ETHBTC
    public Timeframe Timeframe { get; set; }
    public List<DataPoint> DataPoints { get; set; }

    public void AddDataPoint(DataPoint dataPoint) => DataPoints.Add(dataPoint);
}

public class DataPoint {
    public DataPoint(DateTime date, decimal open, decimal high, decimal low, decimal close, decimal spread) {
        Date = date;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Spread = spread;
    }
    public DateTime Date { get; }
    public decimal Open { get; }
    public decimal High { get; }
    public decimal Low { get; }
    public decimal Close { get; }
    public decimal Spread { get; }
}