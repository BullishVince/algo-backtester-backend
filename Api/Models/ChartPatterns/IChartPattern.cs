using AlgoBacktesterBackend.Domain.Models;

namespace AlgoBacktesterBackend.Api.Models.ChartPatterns;
public interface IChartPattern {
    public Task<bool> IsPatternFulfilled(IEnumerable<DataPoint> dataPoints);
}