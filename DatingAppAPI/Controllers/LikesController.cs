using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extentions;
using DatingAppAPI.Helpers;
using DatingAppAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    public class LikesController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            this._likesRepository = likesRepository;
            this._userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            int sourceId = User.GetUserID();
            AppUser user = await this._userRepository.GetUserByUserNameAsync(username);
            AppUser sourceUser = await this._likesRepository.GetUserWithLikes(sourceId);
            if (user == null)
            {
                return NotFound();
            }
            if (sourceUser.UserName.ToLower() == username.ToLower())
            {
                return BadRequest("You cannot like yourself");
            }
            UserLike useToLike = await this._likesRepository.GetUserLike(sourceId, user.Id);
            if (useToLike != null)
            {
                return BadRequest("You already like this user");
            }
            useToLike = new UserLike
            {
                SourceUserId = sourceId,
                TargetUserId = user.Id
            };
            sourceUser.LikedUsers.Add(useToLike);
            if (await this._userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserID();
            PagedList<LikeDTO> users = await this._likesRepository.GetUserLikes
            (likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}