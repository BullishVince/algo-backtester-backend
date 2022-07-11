using Microsoft.AspNetCore.Mvc;
using AlgoBacktesterBackend.Api.Services;

namespace AlgoBacktesterBackend.Api.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class BacktestController : ControllerBase
{

    private readonly ILogger<BacktestController> _logger;
    private readonly IInformationService _informationService;

    public BacktestController(
        ILogger<BacktestController> logger, 
        IInformationService informationService
        )
    {
        _logger = logger;
        _informationService = informationService;
    } 

    [HttpGet]
    public async Task<IActionResult> Backtest()
    {
        var result = await _informationService.GetApiInformation();
        return Ok(result);
    }         
}
