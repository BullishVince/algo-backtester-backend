namespace AlgoBacktesterBackend.Api.Models;
public class TimeframeFilter {
    public string ExecutingTimeframe { get; set; } //Timeframe where trades are taken
    public string MaxTimeframe { get; set; } //The timeframe where analysis starts at (top-down analysis)
    public string[] DisabledTimeframes { get; set; } //Timeframes which you do not want to use for backtesting
}