namespace AlgoBacktesterBackend.Api.Models;
using Skender.Stock.Indicators;
public class Strategy {
    public bool IsHedgingAllowed {get; set;} //Possibility to have both long and short positions open at the same time on the same pair
    public decimal? MaximalRisk {get; set;}
    public TimeframeFilter TimeframeFilter { get; set; }
}