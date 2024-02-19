using Domain.Dtos;

namespace Domain.Interfaces;

public interface IUserDataClient
{
    Task<UserDataClientResult> GetUserAsync(int userId);
}
