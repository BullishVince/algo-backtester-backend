namespace AlgoBacktesterBackend.Domain.Models;
public class Timeframe
{
    public TimeframeType Type { get; set; }
    public int Interval { get; set; }
    public Timeframe(TimeframeType type, int interval)
    {
        Type = type;
        if (type == TimeframeType.Weeks)
        {
            Interval = interval * 7;
        }
        else if (type == TimeframeType.Months)
        {
            Interval = interval * 30;
        } else {
            Interval = interval;
        }
    }
    public Timeframe(string timeframe) {
        switch (timeframe) {
            case "M1":
                Type = TimeframeType.Minutes;
                Interval = 1;
                break;
            case "M5":
                Type = TimeframeType.Minutes;
                Interval = 5;
                break;
            case "M15":
                Type = TimeframeType.Minutes;
                Interval = 15;
                break;
            case "M30":
                Type = TimeframeType.Minutes;
                Interval =30;
                break;
            case "H1":
                Type = TimeframeType.Hours;
                Interval = 1;
                break;
            case "H2":
                Type = TimeframeType.Hours;
                Interval = 2;
                break;
            case "H4":
                Type = TimeframeType.Hours;
                Interval = 4;
                break;
            case "H8":
                Type = TimeframeType.Hours;
                Interval = 8;
                break;
            case "H12":
                Type = TimeframeType.Hours;
                Interval = 12;
                break;
            case "D1":
                Type = TimeframeType.Days;
                Interval =  1;
                break;
            case "W":
                Type = TimeframeType.Weeks;
                Interval = 7;
                break;
            case "M":
                Type = TimeframeType.Months;
                Interval = 30;
                break;
            default:
                Type = TimeframeType.Days;
                Interval = 1;
                break;
        }
    }
}

public enum TimeframeType
{
    Ticks,
    Milliseconds,
    Seconds,
    Minutes,
    Hours,
    Days,
    Weeks,
    Months,
    Years
}