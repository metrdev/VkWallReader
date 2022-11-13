namespace DAL.Interfaces;
public interface IVkParser
{
    Task<string> GetWallAsync(string domain);
}
