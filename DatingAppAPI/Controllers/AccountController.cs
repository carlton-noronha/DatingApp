using System.Security.Cryptography;
using System.Text;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
    public class AccountController: BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService) {
            this._tokenService = tokenService;
            this._context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO credentials) {
            if (await UserExists(credentials.Username)) {
                return BadRequest("Username already exists");
            }
            using HMACSHA512 hmac = new HMACSHA512(); // Class needs to implement IDisposable interface when using the 'using' keyword. The reason for the using statement is to ensure that the object is disposed as soon as it goes out of scope.
            AppUser user = new AppUser {
                UserName = credentials.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password)),
                PasswordSalt = hmac.Key
            };
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();
            return GenerateAuthTokenResponse(user);
        }

        private async Task<bool> UserExists(string username) {
            return await this._context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO credentials) {
            AppUser user = await this._context.Users.Include(user => user.Photos).FirstOrDefaultAsync(user => user.UserName == credentials.Username.ToLower());
            if (user == null) {
                return Unauthorized("Invalid Username!");
            }
            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password));
            for(int i = 0; i < computedHash.Length; ++i) {
                if (computedHash[i] != user.PasswordHash[i]) {
                    return Unauthorized("Invalid Password!");
                }
            }
            return GenerateAuthTokenResponse(user);
        }

        private UserDTO  GenerateAuthTokenResponse(AppUser user) {
            return new UserDTO {
                UserName = user.UserName,
                Token = this._tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url
            };
        }
    }
}