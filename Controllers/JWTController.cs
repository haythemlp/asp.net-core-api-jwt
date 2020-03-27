using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtApplication.Data;
using JwtApplication.Models;
using JwtApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtApplication.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private IAuthentification _userService;
        private readonly UserContext _context;

      

        public JWTController(IAuthentification userService,UserContext context)
        {
            _userService = userService;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public  IActionResult Login([FromBody] RequestLogin userParam)
        {
            var user = _userService.Authenticate(userParam.username, userParam.password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);

        }

     
    }

    public class RequestLogin
    {
       public string username { get; set; }
     public   string password { get; set; }

    }
}