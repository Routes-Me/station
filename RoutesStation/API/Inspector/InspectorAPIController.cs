using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Inspection;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Inspector
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InspectorAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IInspectionRep _inspectionRep;

        public InspectorAPIController(IInspectionRep inspectionRep, UserManager<ApplicationUser> manager)
        {
            _manager = manager;
            _inspectionRep = inspectionRep;
        }
        [HttpPost]
        [Route("ListInspictionBusByInspector")]
        [Produces("application/json")]
        [Authorize(Roles = "Inspector")]
        public async Task<IActionResult> ListPromoterInstallationByPromoter(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _inspectionRep.InspectionBusList();
                result = result.Where(x => x.InspectorID == user.Id).ToList();
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
                        BusID = x.BusID,
                        PalteNumber = x.ApplicationBus!=null?x.ApplicationBus.PalteNumber:null,
                        Kind = x.ApplicationBus != null ? x.ApplicationBus.Kind : null,
                        Company = x.ApplicationBus!=null?x.ApplicationBus.ApplicationCompany!=null?x.ApplicationBus.ApplicationCompany.Company:null:null,
                        Route= x.ApplicationBus != null ? x.ApplicationBus.ApplicationRoute != null ? x.ApplicationBus.ApplicationRoute.Name_EN : null : null,
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
        [Route("ListInspictionBusByInspectorID")]
        [Produces("application/json")]
        [Authorize(Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListInspictionBusByInspectorID(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByIdAsync(pagination.id.ToString());
                var result = await _inspectionRep.InspectionBusList();
                result = result.Where(x => x.InspectorID == user.Id).ToList();
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
                        BusID = x.BusID,
                        PalteNumber = x.ApplicationBus != null ? x.ApplicationBus.PalteNumber : null,
                        Kind = x.ApplicationBus != null ? x.ApplicationBus.Kind : null,
                        Company = x.ApplicationBus != null ? x.ApplicationBus.ApplicationCompany != null ? x.ApplicationBus.ApplicationCompany.Company : null : null,
                        Route = x.ApplicationBus != null ? x.ApplicationBus.ApplicationRoute != null ? x.ApplicationBus.ApplicationRoute.Name_EN : null : null,
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
        [Route("ListInspictionUserByInspector")]
        [Produces("application/json")]
        [Authorize(Roles = "Inspector")]
        public async Task<IActionResult> ListInspictionUserByInspector(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _inspectionRep.InspectionUserList();
                result = result.Where(x => x.InspectorID == user.Id).ToList();
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
                        Name = x.ApplicationUser!=null?x.ApplicationUser.Name:null,
                        Phone = x.ApplicationUser != null ? x.ApplicationUser.PhoneNumber : null,
                        UserID = x.UserID
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
        [Route("ListInspictionUserByInspectorID")]
        [Produces("application/json")]
        [Authorize(Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListInspictionUserByInspectorID(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByIdAsync(pagination.id.ToString());
                var result = await _inspectionRep.InspectionUserList();
                result = result.Where(x => x.InspectorID == user.Id).ToList();
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
                        Name = x.ApplicationUser != null ? x.ApplicationUser.Name : null,
                        Phone = x.ApplicationUser != null ? x.ApplicationUser.PhoneNumber : null,
                        UserID = x.UserID
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