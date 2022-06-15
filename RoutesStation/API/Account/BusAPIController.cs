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
    public class BusAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IBusRep _busRep;

        public BusAPIController(UserManager<ApplicationUser> manager, IBusRep busRep)
        {
            _manager = manager;
            _busRep = busRep;
        }
        [HttpPost]
        [Route("AddBus")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> Add(ApplicationBusView model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var bus = new ApplicationBus
                    {
                       CompanyID=model.CompanyID,
                       RouteID=model.RouteID,
                       PalteNumber=model.PalteNumber,
                       Kind=model.Kind,
                       SocondID=model.SocondID
                     };
                    var result = await _busRep.Add(bus);
                    if (result!=null)
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
        [HttpPost]
        [Route("EditBus")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> Edite(ApplicationBusView model)
        {
            try
            {
                var result = await _busRep.Get(model.id);
                if (result == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "No Bus"
                    });
                }
                if (model.CompanyID != null) result.CompanyID = model.CompanyID;
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> Delete(ApplicationRequestId id)
        {
            try
            {

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
        [AllowAnonymous]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {
            try
            {

                var result = await _busRep.Get(id.id);

                if (result!=null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id=result.id,
                            Active=result.Active,
                            Kind = result.Kind,
                            PalteNumber = result.PalteNumber,
                            RouteID = result.RouteID,
                            RouteName=result.ApplicationRoute!=null?result.ApplicationRoute.Name_EN:null,
                            DriverID = result.DriverID,
                            UserName = result.ApplicationDriver!=null?result.ApplicationDriver.UserName:null,
                            PhoneNumber = result.ApplicationDriver != null ? result.ApplicationDriver.PhoneNumber : null,
                            CompanyID = result.CompanyID,
                            Company=result.ApplicationCompany!=null?result.ApplicationCompany.Company:null,
                            SocondID = result.SocondID,
                             

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
        [HttpGet]
        [Route("ListBus")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _busRep.List();

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x=>new {
                            id=x.id,
                            Route=x.ApplicationRoute.Name_EN,
                            RouteID=x.RouteID,
                            CompanyID = x.CompanyID,
                            Company =x.ApplicationCompany!=null?x.ApplicationCompany.Company:null,
                            PalteNumber = x.PalteNumber,
                            Kind = x.Kind,
                            SocondID = x.SocondID,
                        })
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any buses"
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListBusActive(ApplicationPagination pagination)
        {
            try
            {
                var result = await _busRep.List();
                result = result.Where(x => x.Active).ToList();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new {
                            id = x.id,
                            Route = x.RouteID!=null? x.ApplicationRoute.Name_EN:null,
                            RouteID = x.RouteID,
                            CompanyID = x.CompanyID,
                            Company=x.ApplicationCompany!=null?x.ApplicationCompany.Company:null,
                            PalteNumber = x.PalteNumber,
                            Kind = x.Kind,
                            DeriverID=x.DriverID,
                            Driver=x.DriverID!=null?x.ApplicationDriver.UserName:null,
                            SocondID = x.SocondID,
                        })
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any buses"
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListBusNotActive(ApplicationPagination pagination)
        {
            try
            {
                var result = await _busRep.List();
                result = result.Where(x => !x.Active).ToList();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
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

                        })
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any buses"
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
