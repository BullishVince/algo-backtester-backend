using Microsoft.AspNetCore.Mvc;
using AlgoBacktesterBackend.Api.Services;
using AlgoBacktesterBackend.Api.Models;

namespace AlgoBacktesterBackend.Api.Controllers;
[ApiController]
[Route("[controller]/[action]")]
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

    [HttpGet]
    public async Task<IActionResult> Backtest([FromBody] BacktestingRequest backtestingRequest)
    {
        var result = await _backtestingService.RunBacktest(backtestingRequest);
        return Ok(result);
    }         
}
