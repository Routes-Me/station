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
using RoutesStation.Interface.Inspection;
using RoutesStation.Interface.Wallet;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Wallet
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Inspector")]
    public class WalletByInspectorAPIController : ControllerBase
    {
        private readonly IWalletRep _walletRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IHubContext<DashHub> _hub;
        private readonly IBusRep _busRep;
        private readonly IInspectionRep _inspectionRep;

        public WalletByInspectorAPIController(IWalletRep walletRep, UserManager<ApplicationUser> manager, [NotNull] IHubContext<DashHub> hub,
            IBusRep busRep, IInspectionRep inspectionRep)
        {
            _walletRep = walletRep;
            _manager = manager;
            _hub = hub;
            _busRep = busRep;
            _inspectionRep = inspectionRep;
        }
        
        [HttpPost]
        [Route("ChargeWalletByInspector")]
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
                var result = await _walletRep.ChargeWalletByInspector(cahrgWalletView, user);
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
        [Route("PaymentWalletByInspector")]
        [Produces("application/json")]
        public async Task<IActionResult> PaymentWalletByInspector(ApplicationPaymentWalletView paymentWalletView)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var userPayment = await _manager.FindByIdAsync(paymentWalletView.UserID);
                if (userPayment == null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check User ID"
                    });
                }
                var result = await _walletRep.PaymentWalletByInspector(paymentWalletView, user);
                var Wallet = await _walletRep.Wallet(userPayment);
                var t = await _walletRep.ListPaymentWallet();
                var t1 = await _walletRep.SumPaymentWallet();
                if (result.Status)
                {
                    await _hub.Clients.All.SendAsync("PaymentCount", t.Count().ToString());
                    await _hub.Clients.All.SendAsync("PaymentValueCount", t1.ToString());
                    await _hub.Clients.All.SendAsync("PaymentLive", new { UserName = $"{userPayment.UserName}", Name = $"{userPayment.Name}", Time = $"{DateTime.UtcNow}", Status = true });


                    var payment = await _walletRep.GetPayment(new Guid(result.Message));
                    await _hub.Clients.All.SendAsync("PaymentLiveBus", new { RouteName = $"{payment.ApplicationBus.ApplicationRoute.Name_EN}", UserName = $"{userPayment.UserName}", UserID = $"{Wallet.UserID}", Value = $"{payment.Value}", RouteID = $"{payment.ApplicationBus.RouteID}", Name = $"{userPayment.Name}", Time = $"{DateTime.UtcNow}", CompanyName = $"{payment.ApplicationBus.CompanyID}" });
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

        [HttpPost]
        [Route("CheckPaymentByInspector")]
        [Produces("application/json")]
        public async Task<IActionResult> CheckPaymentByInspector(ApplicationCheckPaymentView checkPaymentView)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _walletRep.CheckPayment(checkPaymentView);
                
                if (result.Status)
                {
                    var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                    DateTimeOffset localServerTime = DateTimeOffset.Now;
                    DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                    var InspectionUser = new ApplicationInspectionUserMap
                    {
                        UserID=checkPaymentView.UserId,
                        InspectorID=user.Id,
                        Creat_Date=localTime.DateTime
                    };
                    await _inspectionRep.AddInspectionUser(InspectionUser);
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            UserName=user.UserName,
                            PaymentDate=result.Message

                        }
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
        [Route("InspectionBus")]
        [Produces("application/json")]
        public async Task<IActionResult> InspectionBus(ApplicationRequestId modle)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _walletRep.ListPaymentWallet();
                result = result.Where(x => x.BusID == modle.id&&x.Payment_Date>=DateTime.Now.Date);
                if (result.Count() > 0)
                {
                    var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                    DateTimeOffset localServerTime = DateTimeOffset.Now;
                    DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                    var InspectionBus = new ApplicationInspectionBusMap
                    {
                        BusID = modle.id,
                        InspectorID = user.Id,
                        Creat_Date = localTime.DateTime
                    };
                    await _inspectionRep.AddInspectionBus(InspectionBus);
                }
                return Ok(new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.id,
                        Value = x.Value,
                        User=x.ApplicationUser!=null?x.ApplicationUser.Name:null,
                        Phone= x.ApplicationUser != null ? x.ApplicationUser.PhoneNumber : null,
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

        [HttpPost]
        [Route("ListCharcheByInspector")]
        [Produces("application/json")]
        public async Task<IActionResult> ListCharcheByPromoter(ApplicationPagination pagination)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var result = await _walletRep.ListChrgingWallet();
                result = result.Where(x => x.InspectorID == user.Id);
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
                        Payment_Date = x.Payment_Date,
                        Value = x.Value.ToString("0.000"),
                        Promoter = x.ApplicationInspector != null ? x.ApplicationInspector.UserName : null,
                        User = x.User != null ? x.User.UserName : null,
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