using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Account;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    public class LgoinAPIController : ControllerBase
    {
        private readonly ILoginRep _loginRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LgoinAPIController(UserManager<ApplicationUser> manager,
            RoleManager<ApplicationRole> roleManage, SignInManager<ApplicationUser> signInManager,
             ILoginRep loginRep)
        {
            _loginRep = loginRep;
            _manager = manager;
            _roleManager = roleManage;
            _signInManager = signInManager;

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ApplicationUserView model)
        {
            try
            {
                var user = await _manager.FindByEmailAsync(model.UserName);
                if (user == null)
                {
                    user = await _manager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        return Ok(new
                        {
                            status = false,
                            description = "Check UserName Or Email"
                        });
                    }

                }
                if ((user.LockoutEnabled == true && user.LockoutEnd > DateTime.UtcNow) || user.PhoneNumberConfirmed == false)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "User InActive"
                    });
                }
                var t1 = await _manager.GetAccessFailedCountAsync(user);
                var role = await _manager.GetRolesAsync(user);
                var result = await _loginRep.LoginUser(model);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            token = result.Message,
                            Type = "Bearer",
                            UserName = user.UserName,
                            id = user.Id,
                            email = user.Email,
                            phoneNumber = user.PhoneNumber,
                            Role = role,
                            AccessFailed = t1,
                            CompanyID = user.CompanyID,
                        }
                    });
                }
                var t = await _manager.AccessFailedAsync(user);
                t1 = await _manager.GetAccessFailedCountAsync(user);
                return Ok(new
                {
                    
                    status = false,
                    description = new
                    {
                       Message= "Check UserName Or Email Or Password",
                        AccessFailed=t1
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }

    }
}