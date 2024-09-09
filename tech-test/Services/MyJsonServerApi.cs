using tech_test.Models;

namespace tech_test.Services;

public class MyJsonServerApi : IMyJsonServerApi
{
    private readonly HttpClient _httpClient;

    public MyJsonServerApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Image?> GetImage(int lastDigitOfUserIdentifier)
    {
        var uri = $"/ck-pacificdev/tech-test/images/{lastDigitOfUserIdentifier}";

        var image = await _httpClient.GetFromJsonAsync<Image>(uri);

        return image;
    }
}
