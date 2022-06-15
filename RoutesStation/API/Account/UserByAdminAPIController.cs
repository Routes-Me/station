using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Account;
using RoutesStation.Interface.Trip;
using RoutesStation.Interface.Wallet;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class UserByAdminAPIController : ControllerBase
    {
        private readonly IRegisterRep _registerRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IWalletRep _walletRep;
        private readonly ITripRep _tripRep;

        public UserByAdminAPIController(IRegisterRep registerRep, UserManager<ApplicationUser> manager, IWalletRep walletRep, ITripRep tripRep)
        {
            _registerRep = registerRep;
            _manager = manager;
            _walletRep = walletRep;
            _tripRep = tripRep;
        }

        [HttpPost]
        [Route("EditeUserByAdmin")]
        [Produces("application/json")]
        public async Task<IActionResult> EditeUser(ApplicationEditeUserView model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _manager.FindByNameAsync(model.id);
                    if (user==null)
                    {
                        return Ok(new
                        {
                            status = true,
                            description = "This User Not Match"
                        });
                    }
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
        [Route("DeactivateUserByAdmin")]
        [Produces("application/json")]
        public async Task<IActionResult> DeactivateUser(ApplicationRequestId id)
        {

            try
            {
                var user = await _manager.FindByIdAsync(id.id.ToString());
                if (user == null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "This User Not Match"
                    });
                }

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

        [HttpPost]
        [Route("ActivateUserByAdmin")]
        [Produces("application/json")]
        public async Task<IActionResult> ActivateUser(ApplicationRequestId id)
        {

            try
            {
                var user = await _manager.FindByIdAsync(id.id.ToString());
                if (user == null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "This User Not Match"
                    });
                }

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

        [HttpPost]
        [Route("ListUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListUser(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.GetUsersInRoleAsync("User");
                var toto = user.Count();
                user = user.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                if (user!=null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = user.Select(x=>new
                        {
                            id=x.Id,
                            UserName=x.UserName,
                            Phone=x.PhoneNumber,
                            Email=x.Email,
                            EmailConfirmed = x.EmailConfirmed,
                            LockoutEnabled = x.LockoutEnabled,
                            LockoutEnd = x.LockoutEnd
                        }),
                        total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "No User In Role User"
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

        [HttpPost]
        [Route("ListEmployee")]
        [Produces("application/json")]
        public async Task<IActionResult> ListEmployee(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.GetUsersInRoleAsync("Employee");
                var toto = user.Count();
                user = user.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                if (user != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = user.Select(x => new
                        {
                            id = x.Id,
                            UserName = x.UserName,
                            Phone = x.PhoneNumber,
                            Email = x.Email,
                            EmailConfirmed = x.EmailConfirmed,
                            LockoutEnabled = x.LockoutEnabled,
                            LockoutEnd = x.LockoutEnd
                        }),
                        total = toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "No User In Role User"
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

        [HttpPost]
        [Route("ListPaymentUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListPaymentUser(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.FindByIdAsync(pagination.id.ToString());
                if (user == null)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "Check User ID"
                    });
                }
                var result = await _walletRep.ListPaymentWalletByUser(pagination);
                if (user != null)
                {
                    return Ok(new
                    {
                        status = true,
                        sumPayment=result.Sum,
                        CountPayment=result.Count,
                        description = result.List.Select(x => new
                        {
                            id = x.id,
                            Value = x.Value,
                            Payment_Date = x.Payment_Date,
                            Route=x.ApplicationBus!=null?x.ApplicationBus.ApplicationRoute!=null?x.ApplicationBus.ApplicationRoute.Name_EN:null:null
                        }),
                        total = result.Count
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "No User In Role User"
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

        [HttpPost]
        [Route("ListChargingUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListChargingUser(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.FindByIdAsync(pagination.id.ToString());
                if (user == null)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "Check User ID"
                    });
                }
                var result = await _walletRep.ListChrgingWalletByUser(pagination);
                
                if (user != null)
                {
                    return Ok(new
                    {
                        status = true,
                        sumCharge = result.Sum,
                        CountCharch=result.Count,
                        description = result.List.Select(x => new
                        {
                            id = x.id,
                            Value = x.Value,
                            Payment_Date = x.Payment_Date,
                            Inspector = x.ApplicationInspector!=null?x.ApplicationInspector.UserName:null,
                            Promoter = x.ApplicationPromoter != null ? x.ApplicationPromoter.UserName : null,

                        }),
                        total = result.Count
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "No User In Role User"
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

        [HttpPost]
        [Route("ListTripUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListTripUser(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.FindByIdAsync(pagination.id.ToString());
                if (user == null)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "Check User ID"
                    });
                }
                var result = await _tripRep.ListByUser(pagination);
                
                if (user != null)
                {
                    return Ok(new
                    {
                        status = true,
                        CountTrip = result.Count,
                        description = result.List.Select(x => new
                        {
                            id = x.id,
                            Route=x.ApplicationRoute!=null?x.ApplicationRoute.Name_EN:null,
                            StartStation = x.StartStation!=null?x.StartStation.Title_EN:null,
                            EndStation = x.EndStation!=null?x.EndStation.Title_EN:null

                        }),
                        total = result.Count
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "No User In Role User"
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