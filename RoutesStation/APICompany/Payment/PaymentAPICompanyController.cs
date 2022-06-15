 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Wallet;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.APICompany.Payment
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
    public class PaymentAPICompanyController : ControllerBase
    {
        private readonly IWalletRep _walletRep;
        private readonly UserManager<ApplicationUser> _manager;

        public PaymentAPICompanyController(UserManager<ApplicationUser> manager, IWalletRep walletRep )
        {
            _walletRep = walletRep;
            _manager = manager;
        }
        [HttpPost]
        [Route("PaymentByCompany")]
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

                var result = await _walletRep.ListPaymentWalletByBusCompany(usercompany.CompanyID.Value);
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
                        Value = x.Value,
                        PalteNumber = x.ApplicationBus!=null?x.ApplicationBus.PalteNumber:null,
                        RouteName = x.ApplicationBus != null ?x.ApplicationBus.ApplicationRoute!=null?x.ApplicationBus.ApplicationRoute.Name_EN:null : null,
                        UserName = x.ApplicationUser!=null?x.ApplicationUser.UserName:null,
                        Date=x.Payment_Date,
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