using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using olappApi.Entities;
using olappApi.Model;

namespace olappApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthController : ControllerBase
    {
        private readonly OlappContext _context;
        public UserAuthController(OlappContext context)
        {
            _context = context;
        }

        public IActionResult Login(UserAuthModel user){
            
            // if(user != null){


            // }
            
            return Ok();
        }



    }
}