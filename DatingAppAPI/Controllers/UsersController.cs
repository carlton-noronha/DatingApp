using AutoMapper;
using CloudinaryDotNet.Actions;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extentions;
using DatingAppAPI.Repositories;
using DatingAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    [Authorize] // Can be added at Method Level
    public class UsersController : BaseAPIController // Will inherit Attributes from BaseAPIController i.e., Route will be /api/Users
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this._photoService = photoService;
            this._mapper = mapper;
            this._userRepository = userRepository;

        }

        //[AllowAnonymous] // If you don't want the endpoint to be authenticated.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            return Ok(await this._userRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await this._userRepository.GetMemberByUserNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            AppUser user = await this._userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }

            this._mapper.Map(memberUpdateDTO, user);

            if (await this._userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to updated user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            AppUser user = await this._userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }
            ImageUploadResult result = await this._photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            Photo photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await this._userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser), new
                {
                    username = user.UserName
                }, this._mapper.Map<PhotoDTO>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            AppUser user = await this._userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }
            Photo photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if (photo == null)
            {
                return NotFound();
            }

            if (photo.IsMain)
            {
                return BadRequest("Already main photo");
            }

            Photo mainPhoto = user.Photos.FirstOrDefault(photo => photo.IsMain);

            if (mainPhoto == null)
            {
                return NotFound();
            }

            mainPhoto.IsMain = false;
            photo.IsMain = true;

            if (await this._userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            AppUser user = await this._userRepository.GetUserByUserNameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }
            Photo photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if (photo == null)
            {
                return NotFound();
            }

            if (photo.IsMain)
            {
                return BadRequest("Cannot delete main photo. Change main photo to another image to delete.");
            }

            if (photo.PublicId != null)
            {
                DeletionResult result = await this._photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) {
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);

            if (await this._userRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Problem deleting photo");
        }
    }

}