using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAppAPI.Data;
using DatingAppAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    public class BuggyController: BaseAPIController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context) {
            this._context = context;
            
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() {
            AppUser user = this._context.Users.Find(-1);
            if(user == null) {
                return NotFound();
            }
            return user;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() {
            AppUser user = this._context.Users.Find(-1);
            return user.ToString();
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() {
            return BadRequest("This was not a good request");
        }

    }
}