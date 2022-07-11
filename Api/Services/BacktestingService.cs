using AlgoBacktesterBackend.Domain.Models;

namespace AlgoBacktesterBackend.Api.Services;
public interface IBacktestingService {
    public Task<BacktestingResult> RunBacktest();
}
public class BacktestingService: IBacktestingService {
    public Task<BacktestingResult> RunBacktest() {
        return Task.FromResult<BacktestingResult>(
            new BacktestingResult()
        );
    }
}