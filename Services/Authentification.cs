using JwtApplication.Data;
using JwtApplication.Helpers;
using JwtApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtApplication.Services
{

    public interface IAuthentification
    {
        Response Authenticate(string username, string password);
        IEnumerable<User> GetAll();


    }

    public class Authentification : IAuthentification
    {


        private readonly AppSettings _appSettings;
        private readonly UserContext _context;

        public Authentification(IOptions<AppSettings> appSettings, UserContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;

        }




        public Response Authenticate(string username, string password)
        {

            var _passwordHasher = new PasswordHasher();
            var user = _context.Users.Include(User => User.Role).FirstOrDefault(x => x.Email == username);

            // return null if user not found
            if (user == null)return null;

            if (!_passwordHasher.Check(user.Password, password)) return null;
        



            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                   
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var authtoken = tokenHandler.CreateToken(tokenDescriptor);


            // remove password before returning
            user.Password = null;

            var respone = new Response { token = tokenHandler.WriteToken(authtoken), user = user };

            return respone;
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _context.Users.ToList(); }


    }

    public class Response
    {

        public string token { get; set; }

        public User user { get; set; }

     
    }
}
