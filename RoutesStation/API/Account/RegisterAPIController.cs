using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Account;
using RoutesStation.Interface.Twillio;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    public class RegisterAPIController : ControllerBase
    {
        private readonly IRegisterRep _registerRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ITwillioRep _twillioRep;

        public RegisterAPIController(IRegisterRep registerRep, UserManager<ApplicationUser> manager, ITwillioRep twillioRep)
        {
            _registerRep = registerRep;
            _manager = manager;
            _twillioRep = twillioRep;
        }

        [HttpPost]
        [Route("Register")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationUserView model)
        {

            if (ModelState.IsValid)
            {
                
                if (await _manager.FindByNameAsync(model.UserName) != null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "This UserName Used in Our System"
                    });

                }

                Random r = new Random();
                int randNum = r.Next(1000000);
                string sixDigitNumber = randNum.ToString("D6");
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        PhoneNumber = model.UserName,
                        Code=sixDigitNumber
                    };
                    var result = await _manager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _manager.AddToRoleAsync(user, "User");
                        await _twillioRep.SendSMS(user.UserName, sixDigitNumber);
                        return StatusCode(200, new
                        {
                            status = true,
                            description = "Success",
                        });
                    }
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Errore"
                    });
                }
                catch(Exception ex)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = ex.ToString()
                    });
                }

            }
            else
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = "Errore"
                });
            }

        }



        


        [HttpPost]
        [Route("ConfirmPhoneNumber")]
        [Produces("application/json")]
        public async Task<IActionResult> Code(ApplicationConfirmPhoneNumberView model)
        {

            if (ModelState.IsValid)
            {
                var user = await _manager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Not Found User"
                    });
                }
                try
                {
                    
                    var result = await _registerRep.ActiveCode(user, model.Code);
                    if (result)
                    {
                        return StatusCode(200, new
                        {
                            status = true,
                            description = "Success",
                        });
                    }
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check Code"
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
            else
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = "Check Code"
                });
            }

        }

        [HttpPost]
        [Route("RestUser")]
        [Produces("application/json")]
        public async Task<IActionResult> RestUser(ApplicationRestUserView model)
        {

            if (ModelState.IsValid)
            {
                var user = await _manager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Not Found User"
                    });
                }
                try
                {
                    var result = await _registerRep.RestUser(model);
                    if (result)
                    {
                        return StatusCode(200, new
                        {
                            status = true,
                            description = "Seccess"
                        });
                    }
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Errore"
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
            else
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = "Check User"
                });
            }

        }

        [HttpPost]
        [Route("EditePassword")]
        [Produces("application/json")]
        public async Task<IActionResult> EditePassword(ApplicationEditePasswordView model)
        {

            if (ModelState.IsValid)
            {
                var user = await _manager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Not Found User"
                    });
                }
                try
                {
                    if (user.Code==model.Code)
                    {
                        var t = await _manager.GeneratePasswordResetTokenAsync(user);
                        var e = await _manager.ResetPasswordAsync(user, t, model.Password);
                        if (e.Succeeded)
                        {
                            await _registerRep.ActiveCode(user, model.Code);
                            return StatusCode(200, new
                            {
                                status = true,
                                description = e.Succeeded
                            });
                        }
                        else
                        {
                            return StatusCode(200, new
                            {
                                status = false,
                                description = e.Succeeded
                            });
                        }
                    }
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check Code"
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
            else
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = "Check Code"
                });
            }

        }

        /*[HttpGet]
        [Route("test")]
        public async Task<IActionResult> GetRole()
        {
            var Role = await _twillioRep.SendSMS("00","0000");
            return Ok(new
            {
                status = true,
                description = Role
            });

        }*/
    }
}