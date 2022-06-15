using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Bus;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class CompanyAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ICompanyRep _companyRep;

        public CompanyAPIController(UserManager<ApplicationUser> manager, ICompanyRep companyRep)
        {
            _manager = manager;
            _companyRep = companyRep;

        }
        [HttpPost]
        [Route("AddUserCompany")]
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
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        PhoneNumber = model.UserName,
                        PhoneNumberConfirmed = true,
                        CompanyID = model.CompanyID,
                    };
                    var result = await _manager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _manager.AddToRoleAsync(user, "Company");

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
        [Route("EditeUserCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationEditeUserView model)
        {

            if (ModelState.IsValid)
            {

                var usercompany = await _manager.FindByIdAsync(model.id);
                if (usercompany == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check User ID",
                    });
                }
                try
                {
                    if (model.CompanyID != null) usercompany.CompanyID = model.CompanyID;
                    var result = await _manager.UpdateAsync(usercompany);
                    if (result.Succeeded)
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
        [Route("GetUserCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(ApplicationRequestId model)
        {

            if (ModelState.IsValid)
            {


                try
                {
                    await _companyRep.List();
                    var driver = await _manager.FindByIdAsync(model.id.ToString());

                    if (driver != null)
                    {
                        return StatusCode(200, new
                        {
                            status = true,
                            description = new
                            {
                                id = driver.Id,
                                UserName = driver.UserName,
                                PhoneNumber = driver.PhoneNumber,
                                CompanyID = driver.CompanyID,
                                Company = driver.ApplicationCompany != null ? driver.ApplicationCompany.Company : null

                            },
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
        [Route("ListUserCompany")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListDriver(ApplicationPagination pagination)
        {
            try
            {
                await _companyRep.List();
                var result = await _manager.GetUsersInRoleAsync("Company");
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
                        PhoneNumber = x.PhoneNumber,
                        CompanyID = x.CompanyID,
                        Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null
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
        [Route("DeleteUserCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteDriver(ApplicationRequestId requestId)
        {
            try
            {
                var user = await _manager.FindByIdAsync(requestId.id.ToString());
                if (user == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "This is not UserCompany"
                    });
                }
                var result = await _manager.DeleteAsync(user);
                if (result.Succeeded)
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

    }
}