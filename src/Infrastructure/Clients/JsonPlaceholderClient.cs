using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Clients;

public class JsonPlaceholderClient : IUserDataClient
{
    private HttpClient _httpClient;

    public JsonPlaceholderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<UserDataClientResult> GetUserAsync(int userId) //TODO: add this to DB
    {
        var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadAsAsync<UserEntity>();

            return new UserDataClientResult
            {
                User = user,
                IsSuccessful = true,
                ErrorMessage = "" //TODO: enrich with status code
            };
        }
        else
        {
            return new UserDataClientResult
            {
                IsSuccessful = false,
                ErrorMessage = response.StatusCode.ToString()
            };
        }
    }
}
