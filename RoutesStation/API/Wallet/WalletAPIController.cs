using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class WalletAPIController : ControllerBase
    {
        private readonly IWalletRep _walletRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IHubContext<DashHub> _hub;
        private readonly IBusRep _busRep;

        public WalletAPIController(IWalletRep walletRep, UserManager<ApplicationUser> manager, [NotNull] IHubContext<DashHub> hub,
            IBusRep busRep)
        {
            _walletRep = walletRep;
            _manager = manager;
            _hub = hub;
            _busRep = busRep;
        }

        [HttpPost]
        [Route("ChargeMyWallet")]
        [Produces("application/json")]
        public async Task<IActionResult> ChargeMyWallet(ApplicationCahrgWalletView cahrgWalletView)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var result = await _walletRep.ChargeWallet(cahrgWalletView,user);
                var Wallet = await _walletRep.Wallet(user);
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
        [Route("PaymentMyWallet")]
        [Produces("application/json")]
        public async Task<IActionResult> PaymentMyWallet(ApplicationPaymentWalletView paymentWalletView)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var result = await _walletRep.PaymentWallet(paymentWalletView, user);
                var Wallet = await _walletRep.Wallet(user);
                var t = await _walletRep.ListPaymentWallet();
                var t1 = await _walletRep.SumPaymentWallet();
                if (result.Status)
                {
                    await _hub.Clients.All.SendAsync("PaymentCount", t.Count().ToString());
                    await _hub.Clients.All.SendAsync("PaymentValueCount", t1.ToString());
                    await _hub.Clients.All.SendAsync("PaymentLive", new { UserName = $"{user.UserName}", Name = $"{user.Name}", Time = $"{DateTime.UtcNow}",Status=true});
                   

                    var payment = await _walletRep.GetPayment(new Guid(result.Message));
                    await _hub.Clients.All.SendAsync("PaymentCountCompany", new {
                        CompanyID = $"{payment.ApplicationBus.CompanyID}" != null ? $"{payment.ApplicationBus.CompanyID}" : null,});
                    await _hub.Clients.All.SendAsync("PaymentLiveBus", new {
                        id = $"{result.Message}",
                        RouteName = $"{payment.ApplicationBus.ApplicationRoute.Name_EN}",
                        UserName = $"{payment.ApplicationUser.UserName}"!=null? $"{payment.ApplicationUser.UserName}":null,
                        Value = $"{payment.Value}",
                        RouteID = $"{payment.ApplicationBus.RouteID}",
                        TripID= $"{payment.TripID}"!=null? $"{payment.TripID}":null,
                        Date = $"{DateTime.UtcNow}",
                        CompanyID = $"{payment.ApplicationBus.CompanyID}" != null ? $"{payment.ApplicationBus.CompanyID}" : null,
                        BusID = $"{payment.BusID}"!=null ? $"{payment.BusID}":null,
                        PalteNumber = $"{payment.BusID}" != null ? $"{payment.ApplicationBus.PalteNumber}" : null,

                    }
                    );
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            PaymentId=result.Message,
                            Total = Wallet.Total.ToString("0.000"),
                            UserName = Wallet.User.UserName,
                            UserId = Wallet.UserID,
                            RouteID=payment!=null?payment.TripID!=null?payment.ApplicationTrip.RouteID:payment.BusID!=null?payment.ApplicationBus.RouteID:null:null,
                            RouteName= payment != null ? payment.TripID != null ? payment.ApplicationTrip.RouteID!=null?payment.ApplicationTrip.ApplicationRoute.Name_EN:null : payment.BusID!=null?payment.ApplicationBus.ApplicationRoute.Name_EN:null : null,


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
        [Route("GetWallet")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            try
            {
                var result = await _walletRep.Wallet(user);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            Total=result.Total.ToString("0.000"),
                            UserName=result.User.UserName,
                            UserId=result.UserID
                        },
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = 0
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
        [Route("SumChargeWallet")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> SumChargeWallet()
        {
            
            try
            {
                var result = await _walletRep.SumChargeWallet();

                if (result != 0)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.ToString("0.000")
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = 0.0
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
        [Route("SumPaymentWallet")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> SumPaymentWallet()
        {

            try
            {
                var result = await _walletRep.SumPaymentWallet();

                if (result != 0)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.ToString("0.000")
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = 0.0
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
        [Route("ListPaymentWalletByUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListPaymentWalletByUser(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _walletRep.ListPaymentWallet();
                result = result.Where(x => x.UserID == user.Id).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.UserID!=null? x.ApplicationUser.UserName:null,
                            Name=x.ApplicationUser!=null?x.ApplicationUser.Name:null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID=x.TripID,
                            RouteID = x != null ? x.TripID != null ? x.ApplicationTrip.RouteID : x.BusID != null ? x.ApplicationBus.RouteID : null : null,
                            RouteName = x != null ? x.TripID != null ? x.ApplicationTrip.RouteID != null ? x.ApplicationTrip.ApplicationRoute.Name_EN : null : x.BusID != null ? x.ApplicationBus.ApplicationRoute.Name_EN : null : null,
                            BusID = x.BusID,
                            PalteNumber = x.BusID!=null?x.ApplicationBus.PalteNumber:null
                        }),total=tot
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListPaymentWalletByBus")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPaymentWalletByDriver(ApplicationPagination pagination)
        {

            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                if (pagination.id == null || pagination.id == Guid.Empty)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Check Bus ID"
                    });
                }
                var bus = await _busRep.Get(pagination.id.Value);
                if (bus.DriverID==null)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "No Driver On Bus Now"
                    });
                }
                var result = await _walletRep.ListPaymentWallet();
                var BusDriver = await _walletRep.GetBusAndDriverActive(pagination.id.Value, user.Id);
                result = result.Where(x => x.DriverID == user.Id && x.BusID==bus.id&&x.Payment_Date>= BusDriver.Start_Date).ToList();
                var tot = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.UserID != null ? x.ApplicationUser.UserName : null,
                            Name = x.ApplicationUser != null ? x.ApplicationUser.Name : null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID = x.TripID,
                            RouteID = x != null ? x.TripID != null ? x.ApplicationTrip.RouteID : x.BusID != null ? x.ApplicationBus.RouteID : null : null,
                            RouteName = x != null ? x.TripID != null ? x.ApplicationTrip.RouteID != null ? x.ApplicationTrip.ApplicationRoute.Name_EN : null : x.BusID != null ? x.ApplicationBus.ApplicationRoute.Name_EN : null : null,
                            BusID = x.BusID,
                            PalteNumber = x.BusID != null ? x.ApplicationBus.PalteNumber : null
                        }),
                        total = tot
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListChrgingWalletByUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListChrgingWalletByUser(ApplicationPagination pagination)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var result = await _walletRep.ListChrgingWallet();
                var tot = result.Count();
                result = result.Where(x => x.UserID == user.Id).ToList();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.User.UserName,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            paymentGateway = x.paymentGateway,
                            invoiceId = x.invoiceId
                        }),total=tot
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListWallet")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListWallet(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListWallet();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.User.UserName,
                            Value = x.Total.ToString("0.000"),
                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListChrgingWallet")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListChrgingWallet(ApplicationPagination pagination)
        {
            
            try
            {
                var result = await _walletRep.ListChrgingWallet();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x=>new
                        {
                            id = x.id,
                            UserName = x.User.UserName,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            paymentGateway = x.paymentGateway,
                            invoiceId = x.invoiceId

                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListChrgingWalletByUserID")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListChrgingWalletByUserID(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListChrgingWallet();
                result = result.Where(x => x.UserID == pagination.id.ToString());
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.User.UserName,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            paymentGateway = x.paymentGateway,
                            invoiceId = x.invoiceId
                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListPaymentWallet")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPaymentWallet(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListPaymentWallet();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName =x.ApplicationUser!=null?x.ApplicationUser.UserName:null,
                            Name = x.ApplicationUser != null ? x.ApplicationUser.Name : null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID = x.TripID,
                            RouteID = x.TripID!=null? x.ApplicationTrip.RouteID:null,
                            RouteName = x.TripID!=null? x.ApplicationTrip.ApplicationRoute.Name_EN:null
                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListPaymentWalletByUserID")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPaymentWalletByUserID(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListPaymentWallet();
                result = result.Where(x => x.UserID == pagination.id.ToString());
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.ApplicationUser != null ? x.ApplicationUser.UserName : null,
                            Name = x.ApplicationUser != null ? x.ApplicationUser.Name : null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID = x.TripID,
                            RouteID = x.ApplicationTrip.RouteID,
                            RouteName = x.ApplicationTrip.ApplicationRoute.Name_EN
                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListPaymentWalletByBusCompany")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPaymentWalletByBusCompany(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListPaymentWalletByBusCompany(pagination.id.Value);
                var CountPayment = result.Count();
                var CountPaymentToday = result.Where(x => x.Payment_Date > DateTime.Today).Count();
                var totalPayment = result.Sum(x => x.Value);
                var totalPaymentToday = result.Where(x => x.Payment_Date > DateTime.Today).Sum(x => x.Value);
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        CountPayment= CountPayment,
                        CountPaymentToday= CountPaymentToday,
                        totalPayment= totalPayment,
                        totalPaymentToday= totalPaymentToday,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.ApplicationUser != null ? x.ApplicationUser.UserName : null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID = x.TripID,
                            RouteID = x.ApplicationBus.RouteID,
                            RouteName = x.ApplicationBus.ApplicationRoute.Name_EN,
                            CompanyName=x.ApplicationBus.ApplicationCompany!=null?x.ApplicationBus.ApplicationCompany.Company:null,
                            CompanyID= x.ApplicationBus.ApplicationCompany != null ? x.ApplicationBus.ApplicationCompany.id.ToString() : null,
                            BusID = x.BusID,
                            PalteNumber = x.BusID != null ? x.ApplicationBus.PalteNumber : null,
                            User=x.ApplicationUser!=null?x.ApplicationUser.UserName:null,

                        }),
                        total = toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("ListPaymentWalletByRouyeID")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListPaymentWalletByRouyeID(ApplicationPagination pagination)
        {

            try
            {
                var result = await _walletRep.ListPaymentWallet();
                
                result = result.Where(x => x.TripID!=null&&x.ApplicationTrip.RouteID==pagination.id).ToList();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x => new
                        {
                            id = x.id,
                            UserName = x.ApplicationUser != null ? x.ApplicationUser.UserName : null,
                            Name = x.ApplicationUser != null ? x.ApplicationUser.Name : null,
                            Value = x.Value.ToString("0.000"),
                            Date = x.Payment_Date,
                            TripID = x.TripID,
                            RouteID = x.ApplicationTrip.RouteID,
                            RouteName = x.ApplicationTrip.ApplicationRoute.Name_EN
                        }),total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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
        [Route("CountCharge")]
        public async Task<IActionResult> CountCharge()
        {
            var count = _walletRep.CountCharge();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count = count.Result,
                },
            });

        }
        [HttpGet]
        [Route("CountPayment")]
        public async Task<IActionResult> CountPayment()
        {
            var count = _walletRep.CountPayment();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count = count.Result,
                },
            });

        }
        [HttpGet]
        [Route("CountPaymentByCompany")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
        public async Task<IActionResult> CountPaymentByCompany()
        {
            var result = await _walletRep.ListPaymentWallet();
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            result = result.Where(x => (x.ApplicationBus != null) ? (x.ApplicationBus.CompanyID == user.CompanyID): x.UserID==user.CompanyID.ToString()|| (x.ApplicationDriver != null) ? (x.ApplicationDriver.CompanyID == user.CompanyID) : x.UserID == user.CompanyID.ToString()).ToList();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count =result.Count(),
                },
            });

        }
        [HttpGet]
        [Route("SumPaymentByCompany")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Company")]
        public async Task<IActionResult> SumPaymentByCompany()
        {
            var result = await _walletRep.ListPaymentWallet();
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            result = result.Where(x => (x.ApplicationBus != null) ? (x.ApplicationBus.CompanyID == user.CompanyID) : x.UserID == user.CompanyID.ToString() || (x.ApplicationDriver != null) ? (x.ApplicationDriver.CompanyID == user.CompanyID) : x.UserID == user.CompanyID.ToString()).ToList();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count = result.Sum(x=>x.Value),
                },
            });

        }
    }
}