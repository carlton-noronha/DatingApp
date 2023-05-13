using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingAppAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppAPI.Services.Implementations
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            SigningCredentials signingCredentails= new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signingCredentails
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}