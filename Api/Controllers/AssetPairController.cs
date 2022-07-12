using Microsoft.AspNetCore.Mvc;
using AlgoBacktesterBackend.Api.Services;

namespace AlgoBacktesterBackend.Api.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class AssetPairController : ControllerBase
{

    private readonly ILogger<AssetPairController> _logger;
    private readonly IAssetPairService _assetPairService;

    public AssetPairController(
        ILogger<AssetPairController> logger, 
        IAssetPairService assetPairService
        )
    {
        _logger = logger;
        _assetPairService = assetPairService;
    } 

    [HttpGet]
    public async Task<IActionResult> GetAssetPairFromFile(string fileName, string timeframe)
    {
        var result = await _assetPairService.GetAssetPairDataFromFile(fileName, timeframe);
        return Ok(result);
    }         
}
