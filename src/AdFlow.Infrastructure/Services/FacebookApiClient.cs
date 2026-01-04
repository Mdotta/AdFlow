using AdFlow.Application.Interfaces;
using System.Net.Http.Json;

namespace AdFlow.Infrastructure.Services;

public class FacebookApiClient : IFacebookApiClient
{
    private readonly HttpClient _httpClient;

    public FacebookApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FacebookUserInfo?> GetUserInfoAsync(string accessToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<FacebookUserInfo>(
                $"https://graph.facebook.com/me?fields=id,email,name,picture&access_token={accessToken}");
            
            return response;
        }
        catch
        {
            return null;
        }
    }
}
