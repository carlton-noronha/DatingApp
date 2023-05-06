using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DatingAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context) {
            if (await context.Users.AnyAsync()) {
                return;
            }
            string userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new DateOnlyJsonConverter());
            List<AppUser> users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            foreach(AppUser user in users) {
                using HMACSHA512 hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}