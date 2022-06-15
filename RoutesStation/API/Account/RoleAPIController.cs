using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Interface.Account;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Account
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin")]
    public class RoleAPIController : ControllerBase
    {
        private readonly IRoleRep _roleRep;
        private readonly RoleManager<ApplicationRole> _roleManage;

        public RoleAPIController(IRoleRep roleRep, RoleManager<ApplicationRole> roleManage)
        {
            _roleRep = roleRep;
            _roleManage = roleManage;
        }
        [HttpPost]
        [Route("AddRole")]
        [Produces("application/json")]
        public async Task<IActionResult> AddRole(ApplicationRoleView model)
        {

            if (ModelState.IsValid)
            {
                var result = await _roleRep.AddRole(model.Role);

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
            return BadRequest();
        }

        [HttpPost]
        [Route("EditeRole")]
        [Produces("application/json")]
        public async Task<IActionResult> EditeRole(ApplicationRoleView model)
        {

            if (ModelState.IsValid)
            {
                var result = await _roleRep.EditeRole(model.id, model.Role);

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
            return BadRequest();
        }

        [HttpPost]
        [Route("RemoveRole")]
        [Produces("application/json")]
        public async Task<IActionResult> RemoveRole(ApplicationRoleView model)
        {

            if (ModelState.IsValid)
            {
                var result = await _roleRep.RemoveRole(model.id);

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
            return BadRequest();
        }

        [HttpGet]
        [Route("ListRole")]
        [AllowAnonymous]
        public async Task<IActionResult> ListRole()
        {
            var Roles = await _roleManage.Roles.ToListAsync();
            Roles = Roles.Where(x => x.NormalizedName != "SUPERADMIN").ToList();
            return Ok(new
            {
                status = true,
                description = Roles
            });

        }

        [HttpPost]
        [Route("GetRole")]
        public async Task<IActionResult> GetRole(ApplicationRequestId requestId)
        {
            var Role =await _roleRep.GetRole(requestId.id.ToString());
            return Ok(new
            {
                status = true,
                description = Role
            });

        }
    }
}