using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using RoutesStation.Interface.PromoterInstallation;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.PromoterInstallation
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PromoterInstallationController : ControllerBase
    {
        private readonly IPromoterInstallationRep _promoterInstallationRep;
        private readonly UserManager<ApplicationUser> _manager;

        public PromoterInstallationController(IPromoterInstallationRep promoterInstallationRep, UserManager<ApplicationUser> manager)
        {
            _promoterInstallationRep = promoterInstallationRep;
            _manager = manager;

        }
        [HttpPost]
        [Route("AddPromoterInstallation")]
        [Produces("application/json")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add(ApplicationPromoterInstallationView promoterInstallationView)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var c = new ApplicationPromoterInstallationMap()
                {
                    PromoterID = promoterInstallationView.PromoterID,
                    UserID = user.Id,
                    Installation_Date = DateTime.Now.Date
                };
                var result = await _promoterInstallationRep.Add(c);
                if (result.Status) return Ok(new
                {
                    status = true,
                    description = result.Message
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Dont Have Any Installiation"
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
        [Route("ListPromoterInstallation")]
        [Produces("application/json")]
        [Authorize(Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPromoterInstallation(ApplicationPagination pagination)
        {
            try
            {
                var result = await _promoterInstallationRep.List();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return StatusCode(200, new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id=x.id,
                        Promoter = x.ApplicationPromoter!=null?x.ApplicationPromoter.UserName:null,
                        PromoterID = x.PromoterID,
                        User=x.ApplicationUser!=null?x.ApplicationUser.UserName:null,
                        UserID = x.UserID,
                        Date=x.Installation_Date
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

        [HttpPost]
        [Route("ListPromoterInstallationByPromoter")]
        [Produces("application/json")]
        [Authorize(Roles = "superAdmin,Admin,Promoter")]
        public async Task<IActionResult> ListPromoterInstallationByPromoter(ApplicationPagination pagination)
        {
            try
            {
                var result = await _promoterInstallationRep.List();
                result = result.Where(x => x.PromoterID == pagination.id.ToString()).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                return StatusCode(200, new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.id,
                        Promoter = x.ApplicationPromoter != null ? x.ApplicationPromoter.UserName : null,
                        PromoterID = x.PromoterID,
                        User = x.ApplicationUser != null ? x.ApplicationUser.UserName : null,
                        UserID = x.UserID,
                        Date = x.Installation_Date
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