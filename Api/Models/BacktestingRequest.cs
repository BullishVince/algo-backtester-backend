namespace AlgoBacktesterBackend.Api.Models;
public class BacktestingRequest {
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal InitialCapital {get; set;}
    public IEnumerable<string> BacktestingPairs {get; set;}
    public Strategy Strategy {get; set;}
}