namespace AlgoBacktesterBackend.Domain.Models;
public class BacktestingStatistics {
    public BacktestingStatistics(DateTime startDate) {
        StartDate = startDate;
    }
    public decimal PercentageGain { get; set; }
    public decimal NetProfit { get; set; }
    public decimal NumberOfTrades {get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfDaysBacktested { get; set; }
    public decimal Profitability { get; set; }
    public decimal AverageWin { get; set; }
    public decimal AverageLoss { get; set; }
    public decimal BestTrade { get; set; }
    public decimal WorstTrade { get; set; }
    public decimal AverageTradeLength { get; set; }
    public decimal ProfitFactor { get; set; }
    public decimal StandardDeviation { get; set; }
    public decimal SharpeRatio { get; set; }
    public decimal Expectancy { get; set; }
    public decimal AHPR { get; set; }
    public decimal GHPR { get; set; }
}