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

        [HttpPost("Login")]
        public IActionResult Login(UserLogInModel user)
        {

            try
            {
                if (user != null)
                {
                    AppUser u = _context.AppUsers.Where(x => x.Username == user.UserName && x.Password == user.Password).FirstOrDefault();

                    if (u == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(u);
                    }
                }

                return NotFound();

            }
            catch (Exception e){
                return BadRequest();
            }


        }



    }
}