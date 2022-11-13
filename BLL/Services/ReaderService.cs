using System.Text;
using System.Text.Json;
using BLL.Dto;
using BLL.Interfaces;
using DAL.Commands.AddCountedWall;
using DAL.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using BLL.Infrastructure;

namespace BLL.Services;
public class ReaderService : IReaderService
{
    private readonly IVkParser _vkParser;
    private readonly IAddCountedWallCommandHandler _addCountedWall;
    private readonly ILogger<ReaderService> _logger;

    public ReaderService(IVkParser vkParser,
        IAddCountedWallCommandHandler addCountedWall, 
        ILogger<ReaderService> logger)
    {
        _vkParser = vkParser;
        _addCountedWall = addCountedWall;
        _logger = logger;
    }

    public async Task<CountedWallDto> CountRepeatedLettersAsync(string domain)
    {
        var wallContent = await RequestData(domain);

        _logger.LogInformation($"Counting letters on page {domain}");
        var sb = new StringBuilder();
        foreach (var item in wallContent.Items)
            sb.Append(item.Text);
        var wallText = sb.ToString().ToLower();

        wallContent.CountedLetters = CountLetters(wallText);
        _logger.LogInformation("Letters were counted");
        wallContent.Domain = domain;

        var model = new AddCountedWallModel
        {
            CountedLetters = JsonDocument.Parse(JsonConvert
                .SerializeObject(wallContent.CountedLetters)),
            Domain = wallContent.Domain,
            Hash = wallContent.GetHashCode()
        };
        await _addCountedWall.AddAsync(model);
        _logger.LogInformation("Result was added to DB");

        return wallContent;
    }

    private static SortedDictionary<char, int> CountLetters(string text)
    {
        var charSet = text
            .Distinct()
            .Where(c => char.IsLetter(c))
            .OrderBy(x => x);
        SortedDictionary<char, int> countedLetters = new();
        foreach (var letter in charSet)
            countedLetters.Add(letter, text.Count(c => c == letter));

        return countedLetters;
    }

    private async Task<CountedWallDto> RequestData(string domain)
    {
        var jsonString = await _vkParser.GetWallAsync(domain);
        var json = JValue.Parse(jsonString);
        var responseToken = json.SelectToken("response");
        if (responseToken == null)
        {
            _logger.LogError("Cannot get data from requested page");
            throw new BadRequestException(json.ToString());
        }
        return JsonConvert
            .DeserializeObject<CountedWallDto>(responseToken.ToString())!;
    }
}
