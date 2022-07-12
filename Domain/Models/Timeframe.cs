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