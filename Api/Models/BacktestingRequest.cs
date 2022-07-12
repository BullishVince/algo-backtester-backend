namespace AlgoBacktesterBackend.Api.Models;
public class BacktestingRequest {
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsHedgingAllowed {get; set;} //Possibility to have both long and short positions open at the same time on the same pair
    public decimal InitialCapital {get; set;}
    public decimal? MaximalRisk {get; set;}
    public IEnumerable<string> BacktestingPairs {get; set;}
    public IEnumerable<string> PositionCriterias {get; set;} //TODO: Think of a better name for this, the criterias which need to be fulfilled in order to execute a trade
}