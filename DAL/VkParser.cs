using DAL.Interfaces;

namespace DAL;
public class VkParser : IVkParser
{
    public async Task<string> GetWallAsync(string domain)
    {
        return await new HttpClient().GetStringAsync(
            $"https://gateway.metrdev.ru/vkapi/getwall/{domain}");
        // Data is requested from custom Minimal API to avoid getting access_token
        // compromised by using public github repository
        // Minimal API code is below:

        /*
        app.MapGet("vkapi/getwall/{domain}", async (string domain) =>
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("https://api.vk.com/method/")
            };
            Dictionary<string, string?> query = new()
            {
                ["access_token"] = token,
                ["domain"] = domain,
                ["count"] = "5",
                ["v"] = "5.131",
            };
            var method = "wall.get";

            return await client.GetStringAsync(QueryHelpers.AddQueryString(method, query));
        });
        */
    }
}
