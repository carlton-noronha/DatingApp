using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    [ApiController] // Helps with Auto Validation, Auto Mapping of Controller Method's Paramters etc
    [Route("api/[controller]")] // example: /api/BaseAPI
    public class BaseAPIController: ControllerBase
    {
        
    }
}