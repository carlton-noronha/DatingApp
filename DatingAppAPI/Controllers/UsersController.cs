using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
    [Authorize] // Can be added at Method Level
    public class UsersController: BaseAPIController // Will inherit Attributes from BaseAPIController i.e., Route will be /api/Users
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this._mapper = mapper;
            
            this._userRepository = userRepository;
            
        }

        //[AllowAnonymous] // If you don't want the endpoint to be authenticated.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers() {
            return Ok(await this._userRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username) {
            return await this._userRepository.GetMemberByUserNameAsync(username);
        }
    }
}