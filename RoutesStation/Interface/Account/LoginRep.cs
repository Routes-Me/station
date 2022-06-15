using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Account
{
	public class LoginRep:ILoginRep
	{
        private readonly IConfiguration _configuration;
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public LoginRep(IConfiguration config, ApplicationDb db, UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _configuration = config;
            _db = db;
            _manager = manager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<StatuseModel> LoginUser(ApplicationUserView loginModel)
        {
            var user = await GetUser(loginModel.UserName, loginModel.Password);
            if (user != null)
            {
                var r = await _manager.GetRolesAsync(user);

                var claims = new List<Claim>();
                claims.Add(new Claim("Name", user.UserName));

                foreach (var cl in r)
                {
                    claims.Add(new Claim("Role", cl));

                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMonths(1), signingCredentials: signIn);

                var statusfinal = new StatuseModel
                {
                    Status = true,
                    Message = new JwtSecurityTokenHandler().WriteToken(token)

                };
                return statusfinal;


            }
            var status = new StatuseModel
            {
                Status = false,
                Message = "This is Error User"

            };
            return status;
        }

        private async Task<ApplicationUser> GetUser(string email, string password)
        {
            var user = await _manager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await _manager.FindByNameAsync(email);
                if (user == null)
                    return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                return user;
            }
            return null;
        }


    }
}

