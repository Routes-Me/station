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

namespace RoutesStation.APICompany.BusCompany
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    public class DriverAPICompanyController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;

        public DriverAPICompanyController(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }
        [HttpPost]
        [Route("DriverByCompany")]
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

                var result = await _manager.GetUsersInRoleAsync("Driver");
                result = result.Where(x => x.CompanyID == usercompany.CompanyID).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.Id,
                        UserName = x.UserName,
                        PhoneNumber = x.PhoneNumber,
                        Name = x.Name,
                        
                    }),total=tot
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