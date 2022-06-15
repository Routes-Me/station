using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Route;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.APICompany.RouteRequest
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    public class RouteRequestAPICompanyController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IRouteRequestRep _routeRequestRep;

        public RouteRequestAPICompanyController(UserManager<ApplicationUser> manager, IRouteRequestRep routeRequestRep)
        {
            _manager = manager;
            _routeRequestRep = routeRequestRep;
        }
        [HttpPost]
        [Route("RequestRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> BusByCompany(ApplicationRouteRequestView routeRequest)
        {
            try
            {
                var usercompany = await _manager.FindByNameAsync(User.Identity.Name);
                if (usercompany.CompanyID == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check Company ID"
                    });
                }
                var req = new ApplicationRouteRequest
                {
                    Name_EN=routeRequest.Name_EN,
                    Name_AR=routeRequest.Name_AR,
                    Area_AR=routeRequest.Area_AR,
                    Area_EN=routeRequest.Area_EN,
                    Price=routeRequest.Price,
                    Read=false,
                    Request_Date=DateTime.Now.Date,
                    AdminID=usercompany.Id,
                    CompanyID=usercompany.CompanyID
                };
                var result = await _routeRequestRep.Add(req);
                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Secsees"
                    });
                }

                return Ok(new
                {
                    status = true,
                    description = "Check Date Enter"
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