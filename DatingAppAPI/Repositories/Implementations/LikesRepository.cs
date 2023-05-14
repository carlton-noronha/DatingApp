using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extentions;
using DatingAppAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dataContext;

        public LikesRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<UserLike> GetUserLike(int sourceId, int targetId)
        {
            return await this._dataContext.Likes.FindAsync(sourceId, targetId);
        }

        public async Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            IQueryable<AppUser> queryUsers = this._dataContext.Users.OrderBy(user => user.UserName).AsQueryable();
            IQueryable<UserLike> queryUserLikes = this._dataContext.Likes.AsQueryable();
            switch (likesParams.Predicate)
            {
                case "liked":
                    queryUserLikes = queryUserLikes.Where(like => like.SourceUserId == likesParams.UserId);
                    queryUsers = queryUserLikes.Select(like => like.TargetUser);
                    break;
                default: // likedBy
                    queryUserLikes = queryUserLikes.Where(like => like.TargetUserId == likesParams.UserId);
                    queryUsers = queryUserLikes.Select(like => like.SourceUser);
                    break;
            }

            IQueryable<LikeDTO> likesUsers = queryUsers.Select(user => new LikeDTO
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeDTO>.CreateAsync(likesUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int id)
        {
            return await this._dataContext.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}