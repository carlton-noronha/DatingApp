using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceId, int targetId);
        Task<AppUser> GetUserWithLikes(int id);
        Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}