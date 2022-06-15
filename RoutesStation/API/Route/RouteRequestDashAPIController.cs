using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Route;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Route
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class RouteRequestDashAPIController : ControllerBase
    {
        private readonly IRouteRequestRep _routeRequestRep;

        public RouteRequestDashAPIController(IRouteRequestRep routeRequestRep)
        {
            _routeRequestRep = routeRequestRep;
        }
        [HttpPost]
        [Route("GetRouteRequest")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRouteRequest(ApplicationRequestId requestId)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var request = await _routeRequestRep.Get(requestId.id);
                if (request == null)
                {
                    return Ok(new
                    {
                        status = true,
                        description ="Check ID"
                    });
                }
                request.Read = true;
                await _routeRequestRep.Edit(request);
                
                return Ok(new
                {
                    status = true,
                    description = new
                    {
                        id=request.id,
                        Name_AR = request.Name_AR,
                        Name_EN = request.Name_EN,
                        Area_AR = request.Area_AR,
                        Area_EN = request.Area_EN,
                        CompanyID = request.CompanyID,
                        Company = request.ApplicationCompany!=null?request.ApplicationCompany.Company:null,
                        AdminID = request.AdminID,
                        Admin = request.ApplicationAdmin!=null?request.ApplicationAdmin.UserName:null,
                        Price = request.Price,
                        Request_Date = request.Request_Date
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

        [HttpPost]
        [Route("UnreadRouteRequest")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> UnreadRouteRequest(ApplicationRequestId requestId)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var request = await _routeRequestRep.Get(requestId.id);
                if (request == null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Check ID"
                    });
                }
                request.Read = false;
                await _routeRequestRep.Edit(request);

                return Ok(new
                {
                    status = true,
                    description = new
                    {
                        id = request.id,
                        Name_AR = request.Name_AR,
                        Name_EN = request.Name_EN,
                        Area_AR = request.Area_AR,
                        Area_EN = request.Area_EN,
                        CompanyID = request.CompanyID,
                        Company = request.ApplicationCompany != null ? request.ApplicationCompany.Company : null,
                        AdminID = request.AdminID,
                        Admin = request.ApplicationAdmin != null ? request.ApplicationAdmin.UserName : null,
                        Price = request.Price,
                        Request_Date = request.Request_Date
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

        [HttpPost]
        [Route("DeletRouteRequest")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletRouteRequest(ApplicationRequestId requestId)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var request = await _routeRequestRep.Get(requestId.id);
                if (request == null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Check ID"
                    });
                }
                
                await _routeRequestRep.Delete(request.id);

                return Ok(new
                {
                    status = true,
                    description = "Seccess"
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
        [Route("DeletMultiRouteRequest")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletMultiRouteRequest(ApplicationRequestId requestId)
        {
            try
            {
                
                await _routeRequestRep.DeleteRange(requestId.MultiID);

                return Ok(new
                {
                    status = true,
                    description = "Seccess"
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
        [Route("ListRouteRequest")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _routeRequestRep.List(pagination);
                
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result.List.Select(x => new { id = x.id, Name = x.Name_AR, Area = x.Area_AR, Company =x.ApplicationCompany!=null?x.ApplicationCompany.Company:null, Price = x.Price,Read=x.Read,Date=x.Request_Date }) :
                        lan == "en" ? result.List.Select(x => new { id = x.id, Name = x.Name_EN, Area = x.Area_EN, Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null, Price = x.Price, Read = x.Read, Date = x.Request_Date }) :
                        result.List.Select(x => new { id = x.id, Name = x.Name_EN, Area = x.Area_EN, Company = x.ApplicationCompany != null ? x.ApplicationCompany.Company : null, Price = x.Price, Read = x.Read, Date = x.Request_Date }),
                        total = result.Count
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any route"
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