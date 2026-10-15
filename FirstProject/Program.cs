using Newtonsoft.Json.Linq;

public class FirstTest
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        Console.WriteLine("This is a test of the first program.");

        using var searchResponse = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
        searchResponse.EnsureSuccessStatusCode();

        var searchContent = await searchResponse.Content.ReadAsStringAsync();

        var user = JObject.Parse(searchContent);
        var city = user.SelectToken("[].name")?.Value<string>()
            ?? throw new InvalidOperationException("The API response did not contain name.");

        Console.WriteLine($"City: {city}");
    }
}