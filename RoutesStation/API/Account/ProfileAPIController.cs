using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProfileAPIController : ControllerBase
    {
        private readonly IRegisterRep _registerRep;
        private readonly UserManager<ApplicationUser> _manager;

        public ProfileAPIController(IRegisterRep registerRep, UserManager<ApplicationUser> manager)
        {
            _registerRep = registerRep;
            _manager = manager;
        }
        [HttpPost]
        [Route("EditeUser")]
        [Produces("application/json")]
        public async Task<IActionResult> EditeUser(ApplicationEditeUserView model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _manager.FindByNameAsync(User.Identity.Name);
                    if (model.Name != null) user.Name = model.Name;
                    if (model.Email != null) user.Email = model.Email;
                    var result = await _registerRep.Edite(user);

                    if (result)
                    {
                        return Ok(new
                        {
                            status = true,
                            description = "Seccess"
                        });
                    }
                    return Ok(new
                    {
                        status = false,
                        description = "Error"
                    });
                }
                catch (Exception ex)
                {
                    return Ok(new
                    {
                        status = false,
                        description = ex.ToString()
                    });
                }

            }
            return BadRequest();
        }

        [HttpPost]
        [Route("EditeImage")]
        [Produces("application/json")]
        public async Task<IActionResult> EditeImage([FromForm]ApplicationImageView model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _manager.FindByNameAsync(User.Identity.Name);
                    
                    var result = await _registerRep.EditeImage(model,user);

                    if (result)
                    {
                        return Ok(new
                        {
                            status = true,
                            description = "Seccess"
                        });
                    }
                    return Ok(new
                    {
                        status = false,
                        description = "Error"
                    });
                }
                catch (Exception ex)
                {
                    return Ok(new
                    {
                        status = false,
                        description = ex.ToString()
                    });
                }

            }
            return BadRequest();
        }

        [HttpGet]
        [Route("DeactivateUser")]
        [Produces("application/json")]
        public async Task<IActionResult> DeactivateUser()
        {

            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);

                var result = await _registerRep.Deactivate(user.Id);

                if (result)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Seccess"
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Error"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }

        [HttpGet]
        [Route("ActivateUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ActivateUser()
        {

            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);

                var result = await _registerRep.Activate(user.Id);

                if (result)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Seccess"
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Error"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }


        [HttpGet]
        [Route("MyProfile")]
        [Produces("application/json")]
        public async Task<IActionResult> MyProfile()
        {

            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);

                

                if (user!=null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            UserName=user.UserName,
                            Phone=user.PhoneNumber,
                            Name=user.Name,
                            Email=user.Email,
                            Image=user.Image,
                        }
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Not Found User"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    description = ex.ToString()
                });
            }
        }

    }
}