using System.ComponentModel.DataAnnotations;

namespace DatingAppAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; } // Property named Id is selected as PK by EFC by default
        
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}