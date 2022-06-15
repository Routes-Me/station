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

namespace RoutesStation.APICompany.BusCompany
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    public class BusAPICompanyController : ControllerBase
    {
        private readonly IBusRep _busRep;
        private readonly UserManager<ApplicationUser> _manager;

        public BusAPICompanyController(IBusRep busRep, UserManager<ApplicationUser> manager)
        {
            _busRep = busRep;
            _manager = manager;
        }
        [HttpPost]
        [Route("BusByCompany")]
        [Produces("application/json")]
        public async Task<IActionResult> BusByCompany(ApplicationPagination pagination)
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

                var result = await _busRep.List();
                result = result.Where(x => x.CompanyID==usercompany.CompanyID).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.id,
                        Kind = x.Kind,
                        PalteNumber = x.PalteNumber,
                        RouteName=x.ApplicationRoute!=null?x.ApplicationRoute.Name_EN:null,
                        Active = x.Active
                    }),total=tot,
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