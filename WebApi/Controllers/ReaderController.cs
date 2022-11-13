using BLL.Dto;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("[controller]/[action]")]
public class ReaderController : ControllerBase
{
    private readonly IReaderService _readerService;

    public ReaderController(IReaderService readerService) => 
        _readerService = readerService;

    /// <summary>
    /// Gets text content from vk wall and counts letters
    /// </summary>
    /// <param name="domain">Address of personal page</param>
    /// <returns>Counted letters and wall text</returns>
    /// <response code="201">Success</response>
    /// <response code="400">If VK API returned any error</response>
    /// <response code="409">If counted letters for this domain already exist</response>
    [HttpGet("{domain}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CountedWallDto>> GetCountedLettersAsync(string domain)
    {
        var result = await _readerService.CountRepeatedLettersAsync(domain);
        return Ok(result);
    }
}
