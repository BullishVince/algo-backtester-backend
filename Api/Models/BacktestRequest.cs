namespace AlgoBacktesterBackend.Api.Models;
public class BacktestRequest {
    IEnumerable<string> BacktestingPairs {get; set;}
    bool IsHedgingAllowed {get; set;} //Possibility to have both long and short positions open at the same time on the same pair
    decimal InitialCapital {get; set;}
    decimal? MaximalRisk {get; set;}
    IEnumerable<string> PositionCriterias {get; set;} //TODO: Think of a better name for this, the criterias which need to be fulfilled in order to execute a trade
}