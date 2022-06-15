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

namespace RoutesStation.APICompany.AddByCompany
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    public class AddBusByCompanyAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IBusRep _busRep;

        public AddBusByCompanyAPIController(UserManager<ApplicationUser> manager, IBusRep busRep)
        {
            _manager = manager;
            _busRep = busRep;
        }
        [HttpPost]
        [Route("AddBus")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationBusView model)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            if (user.CompanyID == null||user.CompanyID==Guid.Empty)
            {
                return StatusCode(200, new
                {
                    status = false,
                    description = "This User not affiliated with company"
                });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var bus = new ApplicationBus
                    {
                        CompanyID = user.CompanyID,
                        RouteID = model.RouteID,
                        PalteNumber = model.PalteNumber,
                        Kind = model.Kind,
                        SocondID = model.SocondID
                    };
                    var result = await _busRep.Add(bus);
                    if (result != null)
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
                        description = "Error"
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
                    description = "Error"
                });
            }

        }

        [Route("EditBus")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationBusView model)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _busRep.Get(model.id);
                if (result == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "No Bus"
                    });
                }
                if (result.CompanyID != user.CompanyID)
                {
                    return StatusCode(403, new
                    {
                        status = false,
                        description = "Limit Access"
                    });
                }
                if (model.Kind != null) result.Kind = model.Kind;
                if (model.PalteNumber != null) result.PalteNumber = model.PalteNumber;
                if (model.RouteID != null) result.RouteID = model.RouteID;
                if (model.SocondID != null) result.SocondID = model.SocondID;
                var result1 = await _busRep.Edite(result);
                if (result != null) return Ok(new
                {
                    status = true,
                    description = result
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Dont Have Any Bus"
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
        [Route("DeleteBus")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(ApplicationRequestId id)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var bus = await _busRep.Get(id.id);
                if (bus == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "No Bus"
                    });
                }
                if (bus.CompanyID != user.CompanyID)
                {
                    return StatusCode(403, new
                    {
                        status = false,
                        description = "Limit Access"
                    });
                }
                var result = await _busRep.Delete(id.id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Message
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
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
        [Route("GetBus")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var bus = await _busRep.Get(id.id);
                if (bus == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "No Bus"
                    });
                }
                if (bus.CompanyID != user.CompanyID)
                {
                    return StatusCode(403, new
                    {
                        status = false,
                        description = "Limit Access"
                    });
                }
                

                if (bus != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id = bus.id,
                            Active = bus.Active,
                            Kind = bus.Kind,
                            PalteNumber = bus.PalteNumber,
                            RouteID = bus.RouteID,
                            RouteName = bus.ApplicationRoute != null ? bus.ApplicationRoute.Name_EN : null,
                            DriverID = bus.DriverID,
                            UserName = bus.ApplicationDriver != null ? bus.ApplicationDriver.UserName : null,
                            PhoneNumber = bus.ApplicationDriver != null ? bus.ApplicationDriver.PhoneNumber : null,
                            CompanyID = bus.CompanyID,
                            Company = bus.ApplicationCompany != null ? bus.ApplicationCompany.Company : null,
                            SocondID = bus.SocondID,


                        }
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Failed"
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
        [Route("ListBus")]
        [Produces("application/json")]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                if (user.CompanyID == null || user.CompanyID == Guid.Empty)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "This User not affiliated with company"
                    });
                }
                var result = await _busRep.List();
                result = result.Where(x => x.CompanyID == user.CompanyID);
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new {
                        id = x.id,
                        Route = x.ApplicationRoute.Name_EN,
                        RouteID = x.RouteID,
                        CompanyID = x.CompanyID,
                        Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null,
                        PalteNumber = x.PalteNumber,
                        Kind = x.Kind,
                        SocondID = x.SocondID,
                    }),
                    total = tot
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
        [Route("ListBusActive")]
        [Produces("application/json")]
        public async Task<IActionResult> ListBusActive(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                if (user.CompanyID == null || user.CompanyID == Guid.Empty)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "This User not affiliated with company"
                    });
                }
                var result = await _busRep.List();
                result = result.Where(x => x.Active&&x.CompanyID==user.CompanyID).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new {
                        id = x.id,
                        Route = x.RouteID != null ? x.ApplicationRoute.Name_EN : null,
                        RouteID = x.RouteID,
                        CompanyID = x.CompanyID,
                        Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null,
                        PalteNumber = x.PalteNumber,
                        Kind = x.Kind,
                        DeriverID = x.DriverID,
                        Driver = x.DriverID != null ? x.ApplicationDriver.UserName : null,
                        SocondID = x.SocondID,
                    }),
                    total = tot
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
        [Route("ListBusNotActive")]
        [Produces("application/json")]
        public async Task<IActionResult> ListBusNotActive(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                if (user.CompanyID == null || user.CompanyID == Guid.Empty)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "This User not affiliated with company"
                    });
                }
                var result = await _busRep.List();
                result = result.Where(x => !x.Active&&x.CompanyID==user.CompanyID).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new {
                        id = x.id,
                        Route = x.ApplicationRoute.Name_EN,
                        RouteID = x.RouteID,
                        CompanyID = x.CompanyID,
                        Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null,
                        PalteNumber = x.PalteNumber,
                        Kind = x.Kind,
                        SocondID = x.SocondID,

                    }),
                    total = tot
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