using BLL.Dto;

namespace BLL.Interfaces;
public interface IReaderService
{
    Task<CountedWallDto> CountRepeatedLettersAsync(string domain);
}
