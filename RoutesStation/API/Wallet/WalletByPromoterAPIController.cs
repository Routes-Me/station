using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RoutesStation.Hubs;
using RoutesStation.Interface.Bus;
using RoutesStation.Interface.Wallet;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Wallet
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Promoter")]
    public class WalletByPromoterAPIController : ControllerBase
    {
        private readonly IWalletRep _walletRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IHubContext<DashHub> _hub;
        private readonly IBusRep _busRep;

        public WalletByPromoterAPIController(IWalletRep walletRep, UserManager<ApplicationUser> manager, [NotNull] IHubContext<DashHub> hub,
            IBusRep busRep)
        {
            _walletRep = walletRep;
            _manager = manager;
            _hub = hub;
            _busRep = busRep;
        }
        [HttpPost]
        [Route("ChargeWalletByPromoter")]
        [Produces("application/json")]
        public async Task<IActionResult> ChargeMyWallet(ApplicationCahrgWalletView cahrgWalletView)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var usercharche = await _manager.FindByIdAsync(cahrgWalletView.UserID);
                if (usercharche == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check User ID"
                    });
                }
                var result = await _walletRep.ChargeWalletByPromoter(cahrgWalletView, user);
                var Wallet = await _walletRep.Wallet(usercharche);
                var t = await _walletRep.ListChrgingWallet();
                var t1 = await _walletRep.SumChargeWallet();
                if (result.Status)
                {
                    await _hub.Clients.All.SendAsync("ChargeCount", t.Count().ToString());
                    await _hub.Clients.All.SendAsync("ChargeValueCount", t1.ToString());
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            Total = Wallet.Total.ToString("0.000"),
                            UserName = Wallet.User.UserName,
                            UserId = Wallet.UserID
                        },
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
        [Route("ListCharcheByPromoter")]
        [Produces("application/json")]
        public async Task<IActionResult> ListCharcheByPromoter(ApplicationPagination pagination)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var result = await _walletRep.ListChrgingWallet();
                result = result.Where(x => x.PromoterID == user.Id);
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
                        Payment_Date = x.Payment_Date,
                        Value = x.Value.ToString("0.000"),
                        Promoter = x.ApplicationPromoter!=null?x.ApplicationPromoter.UserName:null,
                        User=x.User!=null?x.User.UserName:null,
                    })
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