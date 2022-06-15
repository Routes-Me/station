using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class AdminAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;

        public AdminAPIController(UserManager<ApplicationUser> manager)
        {
            _manager = manager;

        }

        [HttpPost]
        [Route("AddAdmin")]
        [Produces("application/json")]
        public async Task<IActionResult> AddAdmin(ApplicationUserAdmin model)
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


                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.email,
                        PhoneNumberConfirmed = true
                    };
                    var result = await _manager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _manager.AddToRoleAsync(user, "Admin");

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
                    description = "Errore"
                });
            }

        }

        [HttpPost]
        [Route("AddPromoter")]
        [Produces("application/json")]
        public async Task<IActionResult> AddPromoter(ApplicationUserAdmin model)
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
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.email,
                        PhoneNumberConfirmed = true
                    };
                    var result = await _manager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _manager.AddToRoleAsync(user, "Promoter");

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
                    description = "Errore"
                });
            }

        }


        [HttpPost]
        [Route("AddInspector")]
        [Produces("application/json")]
        public async Task<IActionResult> AddInspector(ApplicationUserAdmin model)
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
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.email,
                        PhoneNumberConfirmed = true
                    };
                    var result = await _manager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _manager.AddToRoleAsync(user, "Inspector");

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
                    description = "Errore"
                });
            }

        }

        [HttpPost]
        [Route("ListPromoter")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListDriver(ApplicationPagination pagination)
        {
            try
            {
                var result = await _manager.GetUsersInRoleAsync("Promoter");
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return StatusCode(200, new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber
                    }),
                    total = toto
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

        [HttpPost]
        [Route("ListInspector")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListInspector(ApplicationPagination pagination)
        {
            try
            {
                var result = await _manager.GetUsersInRoleAsync("Inspector");
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return StatusCode(200, new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber
                    }),
                    total = toto
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