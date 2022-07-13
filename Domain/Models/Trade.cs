namespace AlgoBacktesterBackend.Domain.Models;
public class Trade {
    private Trade (DateTime openDate, string symbol, string action, decimal units, decimal? stopLoss,decimal? takeProfit, decimal openPrice) {
        OpenDate = openDate;
        Symbol = symbol;
        Action = action;
        Units = units;
        StopLoss = stopLoss;
        TakeProfit = takeProfit;
        OpenPrice = openPrice;
        Id = Guid.NewGuid();
    }
    public Guid Id { get; }
    public DateTime OpenDate { get; }
    public DateTime? CloseDate { get; set; }
    public string Symbol { get;}
    public string Action { get;}
    public decimal Units { get;} //Expressed as Lots in fx where 1 lot = 100 000 units of the given currency
    public decimal? StopLoss { get;}
    public decimal? TakeProfit { get;}
    public decimal OpenPrice { get;}
    public decimal ClosePrice { get; set; }
    public decimal Pips { get; set; }
    public decimal NetProfit { get; set; }
    public double Duration { get; set; }
    public decimal Gain { get; set; } //Expressed as a percentage of the current capital

    public static Trade OpenTrade(DateTime openDate, string symbol, string action, decimal units, decimal? stopLoss,decimal? takeProfit, decimal openPrice) {
        return new Trade(openDate, symbol, action, units, stopLoss, takeProfit, openPrice);
    }

    public void CloseTrade(DateTime closeDate, decimal closePrice, decimal currentCapital) {
        ClosePrice = closePrice;
        CloseDate = closeDate;
        Duration = CloseDate.GetValueOrDefault().Subtract(OpenDate).TotalMinutes;

        //Calculation a bit longer since we also want short winning positions to yield a positive value
        Pips = Action == "BUY" ? closePrice - OpenPrice : OpenPrice - closePrice;

        NetProfit = CalculateNetProfit();
        Gain = (NetProfit/currentCapital)*100;
    }

    private decimal CalculateNetProfit() {
        var underlyingPosition = Units*100000;
        var pipValue = underlyingPosition*OpenPrice;
        return pipValue*Pips;
    }
}