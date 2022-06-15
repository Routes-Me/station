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

namespace RoutesStation.API.Bus
{
    [Route("api/divice")]
    [ApiController]
    public class BusDiviceAPIController : ControllerBase
    {
        private readonly IBusRep _busRep;
        private readonly IWalletRep _walletRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IHubContext<DashHub> _hub;

        public BusDiviceAPIController(IBusRep busRep, IWalletRep walletRep, UserManager<ApplicationUser> manager,[NotNull] IHubContext<DashHub> hub)
        {
            _busRep = busRep;
            _walletRep = walletRep;
            _manager = manager;
            _hub = hub;
        }
        [HttpPost]
        [Route("ActiveBus")]
        [Produces("application/json")]
        public async Task<IActionResult> DriverEnter(ApplicationSecondID secondID)
        {
            
            try
            {
                var test = await _busRep.ActiveBus(secondID);
                if (test.Status)
                {
                    var result = await _busRep.Get(new Guid(test.Message));
                    return StatusCode(200, new
                    {
                        status = true,
                        description = new
                        {
                            id = result.id,
                            Active = result.Active,
                            Kind = result.Kind,
                            PalteNumber = result.PalteNumber,
                            RouteID = result.RouteID,
                            RouteName = result.ApplicationRoute != null ? result.ApplicationRoute.Name_EN : null,
                            Distination = result.ApplicationRoute != null ? result.ApplicationRoute.From_To_EN : null,
                            Price = result.ApplicationRoute != null ? result.ApplicationRoute.Price : 0,
                            DriverID = result.DriverID,
                            UserName = result.ApplicationDriver != null ? result.ApplicationDriver.UserName : null,
                            PhoneNumber = result.ApplicationDriver != null ? result.ApplicationDriver.PhoneNumber : null,
                            CompanyID = result.CompanyID,
                            Company = result.ApplicationCompany != null ? result.ApplicationCompany.Company : null,
                            SocondID = result.SocondID,


                        }
                    });
                }
                return StatusCode(200, new
                {
                    status = false,
                    description = test.Message
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
        [Route("UnActiveBus")]
        [Produces("application/json")]
        public async Task<IActionResult> DriverOut(ApplicationSecondID model)
        {
            try
            {
                var test = await _busRep.UnActiveBus(model);
                return StatusCode(200, new
                {
                    status = true,
                    description = test
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
        [Route("PaymentBySecondID")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentBySecondID(ApplicationPaymentBySecondID paymentBySecondID)
        {
            
            try
            {
                var user = await _manager.FindByIdAsync(paymentBySecondID.UserID);
                if (user == null)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "Check User Name"
                    });
                }
                var result = await _walletRep.PaymentBySecondID(paymentBySecondID, user);
                var Wallet = await _walletRep.Wallet(user);
                var t = await _walletRep.ListPaymentWallet();
                var t1 = await _walletRep.SumPaymentWallet();
                if (result.Status)
                {
                    user.PaymentCode = null;
                    await _manager.UpdateAsync(user);
                    var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                    DateTimeOffset localServerTime = DateTimeOffset.Now;
                    DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);

                    await _hub.Clients.All.SendAsync("PaymentCount", t.Count().ToString());
                    await _hub.Clients.All.SendAsync("PaymentValueCount", t1.ToString());
                    await _hub.Clients.All.SendAsync("PaymentLive", new { UserName = $"{user.UserName}", Name = $"{user.Name}", Time = $"{localServerTime.DateTime}", Status = true });


                    var payment = await _walletRep.GetPayment(new Guid(result.Message));
                    await _hub.Clients.All.SendAsync("PaymentLiveBus", new
                    {
                        id = $"{result.Message}",
                        RouteName = $"{payment.ApplicationBus.ApplicationRoute.Name_EN}",
                        UserName = $"{payment.ApplicationUser.UserName}" != null ? $"{payment.ApplicationUser.UserName}" : null,
                        Value = $"{payment.Value}",
                        RouteID = $"{payment.ApplicationBus.RouteID}",
                        TripID = $"{payment.TripID}" != null ? $"{payment.TripID}" : null,
                        Date = $"{DateTime.UtcNow}",
                        CompanyID = $"{payment.ApplicationBus.CompanyID}" != null ? $"{payment.ApplicationBus.CompanyID}" : null,
                        BusID = $"{payment.BusID}" != null ? $"{payment.BusID}" : null,
                        PalteNumber = $"{payment.BusID}" != null ? $"{payment.ApplicationBus.PalteNumber}" : null,

                    }
                    );
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            PaymentId = result.Message,
                            Total = Wallet.Total.ToString("0.000"),
                            UserName = Wallet.User.UserName,
                            UserId = Wallet.UserID,
                            RouteID = payment != null ? payment.TripID != null ? payment.ApplicationTrip.RouteID : payment.BusID != null ? payment.ApplicationBus.RouteID : null : null,
                            RouteName = payment != null ? payment.TripID != null ? payment.ApplicationTrip.RouteID != null ? payment.ApplicationTrip.ApplicationRoute.Name_EN : null : payment.BusID != null ? payment.ApplicationBus.ApplicationRoute.Name_EN : null : null,


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

        [HttpGet]
        [Route("PaymentCode")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PaymentCode()
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check UserID"
                    });
                }
                string code = Guid.NewGuid().ToString().Substring(0, 8);
                user.PaymentCode = code;
                var test = await _manager.UpdateAsync(user);
                if (test.Succeeded)
                {
                    return StatusCode(200, new
                    {
                        status = true,
                        description = new
                        {
                            PaymentCode= code
                        }
                    });
                }
                else
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Errore"
                    });
                }
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