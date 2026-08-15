using DelightBistroMvc.Data.Models;

namespace DelightBistroMinimalApi.Services.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateToken(UserData user);
    }
}
