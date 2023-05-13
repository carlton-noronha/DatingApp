using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Repositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
        Task<MemberDTO> GetMemberByUserNameAsync(string username);
    }
}