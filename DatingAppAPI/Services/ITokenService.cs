using DatingAppAPI.Entities;

namespace DatingAppAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}