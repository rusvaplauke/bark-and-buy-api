using Domain.Entities;

namespace Domain.Dtos;

public class UserDataClientResult
{
    public UserEntity User { get; set; } = new UserEntity();
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
}
