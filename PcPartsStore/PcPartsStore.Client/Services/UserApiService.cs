using Shared.Dto;
using System.Net.Http.Json;

namespace PcPartsStore.Client.Services;

public class UserApiService : IUserApiService
{
    private readonly HttpClient _httpClient;

    public UserApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> GetUserRoleAsync()
    {
        var response = await _httpClient.GetAsync("api/UserManagerDomain/role");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error fetching user role: {errorMessage}");
            return new List<string>();
        }

        return await response.Content.ReadFromJsonAsync<List<string>>();
    }


    public async Task<UserInfoDto> GetUserInfoAsync()
    {
        return await _httpClient.GetFromJsonAsync<UserInfoDto>("api/UserManagerDomain/info");
    }
}
