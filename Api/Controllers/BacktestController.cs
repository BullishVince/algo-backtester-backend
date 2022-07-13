using Microsoft.AspNetCore.Mvc;
using AlgoBacktesterBackend.Api.Services;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Api.Config.Messages;
using AlgoBacktesterBackend.Domain.Models;

namespace AlgoBacktesterBackend.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class BacktestController : ControllerBase
{

    private readonly ILogger<BacktestController> _logger;
    private readonly IBacktestingService _backtestingService;

    public BacktestController(
        ILogger<BacktestController> logger, 
        IBacktestingService backtestingService
        )
    {
        _logger = logger;
        _backtestingService = backtestingService;
    } 

    [HttpPost]
    public async Task<IResponseMessage<BacktestingStatistics>> Backtest([FromBody] BacktestingRequest backtestingRequest) {
        return await _backtestingService.Backtest(backtestingRequest); 
    }    
}
