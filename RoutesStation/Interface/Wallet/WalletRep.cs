using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;
using Microsoft.AspNetCore.SignalR;
using RoutesStation.Hubs;
using RoutesStation.ModelsView;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using RoutesStation.ModelReturn;

namespace RoutesStation.Interface.Wallet
{
	public class WalletRep:IWalletRep
	{
        private readonly ApplicationDb _db;
        private readonly IHubContext<DashHub> _hub;
        private readonly string _api_key;
        private readonly string _api_secret;
        private readonly UserManager<ApplicationUser> _manager;

        public WalletRep(ApplicationDb db,[NotNull] IHubContext<DashHub> hub,UserManager<ApplicationUser> manager)
		{
            _db = db;
            _hub = hub;
            _api_key = "$FhlF]3;.OIic&{>H;_DeW}|:wQ,A8";
            _api_secret = "Z~P7-_/i!=}?BIwAd*S67LBzUo4O^G";
            _manager = manager;

        }

        public async Task<StatuseModel> ChargeWallet(ApplicationCahrgWalletView cahrgWalletView, ApplicationUser user)
        {
            if (!_api_key.Equals(cahrgWalletView.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (!_api_secret.Equals(cahrgWalletView.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (cahrgWalletView.invoiceValue <= 0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User must be Null"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == user.Id);
            var Wcharge = new ApplicationWalletCharging
            {
                Value = cahrgWalletView.invoiceValue,
                Payment_Date = localTime.DateTime,
                UserID = user.Id,
                invoiceId=cahrgWalletView.invoiceId,
                paymentGateway=cahrgWalletView.paymentGateway,
                TypeCharge= TypeCharge.cart,
            };
            _db.WalletChargings.Add(Wcharge);
            
            if (Wallet != null)
            {
                _db.Wallets.Attach(Wallet);
                Wallet.Total = Wallet.Total+cahrgWalletView.invoiceValue;
            }
            else
            {
                var wal = new ApplicationWallet
                {
                    Total=cahrgWalletView.invoiceValue,
                    UserID=user.Id
                };
                _db.Wallets.Add(wal);
            }
            await _db.SaveChangesAsync();
            var Seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
         
            return Seccess;
        }

        public async Task<StatuseModel> ChargeWalletByInspector(ApplicationCahrgWalletView cahrgWalletView, ApplicationUser user)
        {
            if (!_api_key.Equals(cahrgWalletView.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (!_api_secret.Equals(cahrgWalletView.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (cahrgWalletView.invoiceValue <= 0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Promoter  Null"
                };
                return faild;
            }
            var usercharge = await _manager.FindByIdAsync(cahrgWalletView.UserID);
            if (usercharge == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User  Null"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var WalletInspector = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == user.Id);
            if (WalletInspector == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Inspector Don't Have Wallet"
                };
                return faild;
            }
            if (WalletInspector.Total < cahrgWalletView.invoiceValue)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Inspector Don't Have enghe plance"
                };
                return faild;
            }
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == usercharge.Id);
            var Wcharge = new ApplicationWalletCharging
            {
                Value = cahrgWalletView.invoiceValue,
                Payment_Date = localTime.DateTime,
                UserID = usercharge.Id,
                invoiceId = cahrgWalletView.invoiceId,
                paymentGateway = cahrgWalletView.paymentGateway,
                TypeCharge = TypeCharge.Inspector,
                InspectorID = user.Id
            };
            _db.WalletChargings.Add(Wcharge);

            _db.Wallets.Attach(WalletInspector);
            WalletInspector.Total = WalletInspector .Total - cahrgWalletView.invoiceValue;
            if (Wallet != null)
            {
                _db.Wallets.Attach(Wallet);
                Wallet.Total = Wallet.Total + cahrgWalletView.invoiceValue;
            }
            else
            {
                var wal = new ApplicationWallet
                {
                    Total = cahrgWalletView.invoiceValue,
                    UserID = usercharge.Id
                };
                _db.Wallets.Add(wal);
            }
            await _db.SaveChangesAsync();
            var Seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };

            return Seccess;
        }

        public async Task<StatuseModel> ChargeWalletByPromoter(ApplicationCahrgWalletView cahrgWalletView, ApplicationUser user)
        {
            if (!_api_key.Equals(cahrgWalletView.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (!_api_secret.Equals(cahrgWalletView.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused charging"
                };
                return faild;
            }
            if (cahrgWalletView.invoiceValue <= 0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Promoter  Null"
                };
                return faild;
            }
            var usercharge = await _manager.FindByIdAsync(cahrgWalletView.UserID);
            if (usercharge == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User  Null"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var WalletPrometor=await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == user.Id);
            if (WalletPrometor==null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Prometor Don't Have Wallet"
                };
                return faild;
            }
            if(WalletPrometor.Total< cahrgWalletView.invoiceValue)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Prometor Don't Have enghe plance"
                };
                return faild;
            }
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == usercharge.Id);
            var Wcharge = new ApplicationWalletCharging
            {
                Value = cahrgWalletView.invoiceValue,
                Payment_Date = localTime.DateTime,
                UserID = usercharge.Id,
                invoiceId = cahrgWalletView.invoiceId,
                paymentGateway = cahrgWalletView.paymentGateway,
                TypeCharge = TypeCharge.Promoter,
                PromoterID=user.Id
            };
            _db.WalletChargings.Add(Wcharge);

            _db.Wallets.Attach(WalletPrometor);
            WalletPrometor.Total = WalletPrometor.Total - cahrgWalletView.invoiceValue;
            if (Wallet != null)
            {
                _db.Wallets.Attach(Wallet);
                Wallet.Total = Wallet.Total + cahrgWalletView.invoiceValue;
            }
            else
            {
                var wal = new ApplicationWallet
                {
                    Total = cahrgWalletView.invoiceValue,
                    UserID = usercharge.Id
                };
                _db.Wallets.Add(wal);
            }
            await _db.SaveChangesAsync();
            var Seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };

            return Seccess;

        }

        public async Task<StatuseModel> CheckPayment(ApplicationCheckPaymentView checkPaymentView)
        {
            var user = await _manager.FindByIdAsync(checkPaymentView.UserId);
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "Check User ID"
                };
                return faild;
            }
            var data = await _db.WalletTrips.FirstOrDefaultAsync(x=>x.id==checkPaymentView.PaymentId&&x.UserID==checkPaymentView.UserId);
            if (data != null)
            {
                var Seccess = new StatuseModel
                {
                    Status = true,
                    Message = data.Payment_Date.ToString()
                };
                return Seccess;
            }
            var faild1 = new StatuseModel
            {
                Status = false,
                Message = "No Payment"
            };
            return faild1;
        }

        public async Task<int> CountCharge()
        {
            return await _db.WalletChargings.OrderBy(x => x.id).CountAsync();
        }

        public async Task<int> CountPayment()
        {
            return await _db.WalletTrips.OrderBy(x => x.id).CountAsync(); ;
        }

        public async Task<ApplicationBusDriverMap> GetBusAndDriverActive(Guid BusID, string DriverID)
        {
            return await _db.BusDriverMaps.Include(x => x.ApplicationBus).Include(x => x.ApplicationDriver).OrderBy(x => x.Start_Date).
                LastOrDefaultAsync(x => x.BusID == BusID && x.DriverID == DriverID.ToString());
        }

        public async Task<ApplicationWalletTrip> GetPayment(Guid PaymentID)
        {
            return await _db.WalletTrips.Include(x=>x.ApplicationBus).ThenInclude(x=>x.ApplicationRoute).Include(x => x.ApplicationTrip).ThenInclude(x => x.ApplicationRoute).FirstOrDefaultAsync(x => x.id == PaymentID);
        }

        public async Task<IEnumerable<ApplicationWalletCharging>> ListChrgingWallet()
        {
            return await _db.WalletChargings.Include(x => x.User).Include(x=>x.ApplicationPromoter).Include(x=>x.ApplicationInspector).OrderByDescending(x => x.Payment_Date).ToListAsync();
        }

        public async Task<ApplicationGeneralListWithCountSum<ApplicationWalletCharging>> ListChrgingWalletByUser(ApplicationPagination pagination)
        {
            var sum = await _db.WalletChargings.Where(x => x.UserID == pagination.id.ToString()).SumAsync(x => x.Value);
            var Count = await _db.WalletChargings.Where(x => x.UserID == pagination.id.ToString()).CountAsync();

            var Listret= await _db.WalletChargings
                .Include(x => x.User).Include(x => x.ApplicationPromoter)
                .Include(x => x.ApplicationInspector).Where(x=>x.UserID==pagination.id.ToString()).OrderByDescending(x => x.Payment_Date).Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToListAsync();
            return new ApplicationGeneralListWithCountSum<ApplicationWalletCharging>
            {
                Sum=sum,
                Count=Count,
                List=Listret
            };
        }

        public async Task<IEnumerable<ApplicationWalletTrip>> ListPaymentWallet()
        {
            return await _db.WalletTrips.Include(x => x.ApplicationUser)
                .Include(x=>x.ApplicationInspector).Include(x=>x.ApplicationTrip).ThenInclude(x=>x.ApplicationRoute)
                .Include(x=>x.ApplicationBus).ThenInclude(x=>x.ApplicationRoute).Include(x=>x.ApplicationDriver)
                .OrderByDescending(x => x.Payment_Date).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationWalletTrip>> ListPaymentWalletByBusCompany(Guid CompanyID)
        {
            return await _db.WalletTrips.Include(x => x.ApplicationUser)
                .Include(x=>x.ApplicationInspector).Include(x => x.ApplicationTrip).ThenInclude(x => x.ApplicationRoute)
               .Include(x => x.ApplicationBus).ThenInclude(x => x.ApplicationRoute).Include(x => x.ApplicationDriver).Include(x=>x.ApplicationBus).ThenInclude(x=>x.ApplicationCompany)
               .OrderByDescending(x => x.Payment_Date).Where(x=>x.ApplicationBus.CompanyID==CompanyID).ToListAsync();
        }

        public async Task<ApplicationGeneralListWithCountSum<ApplicationWalletTrip>> ListPaymentWalletByUser(ApplicationPagination pagination)
        {
            var sum = await _db.WalletTrips.Where(x => x.UserID == pagination.id.ToString()).SumAsync(x => x.Value);
            var Count = await _db.WalletTrips.Where(x => x.UserID == pagination.id.ToString()).CountAsync();
            var Listret= await _db.WalletTrips.Include(x => x.ApplicationUser)
                .Include(x => x.ApplicationInspector).Include(x => x.ApplicationTrip).ThenInclude(x => x.ApplicationRoute)
                .Include(x => x.ApplicationBus).ThenInclude(x => x.ApplicationRoute).Include(x => x.ApplicationDriver)
                .Where(x=>x.UserID==pagination.id.ToString())
                .OrderByDescending(x => x.Payment_Date).Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToListAsync();
            var ret = new ApplicationGeneralListWithCountSum<ApplicationWalletTrip>
            {
                Sum = sum,
                Count = Count,
                List = Listret
            };
            return ret;
        }

        public async Task<IEnumerable<ApplicationWallet>> ListWallet()
        {
            return await _db.Wallets.Include(x => x.User).OrderByDescending(x => x.User.UserName).ToListAsync();
        }

        public async Task<StatuseModel> PaymentBySecondID(ApplicationPaymentBySecondID paymentBySecondID, ApplicationUser user)
        {
            if (!_api_key.Equals(paymentBySecondID.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (!_api_secret.Equals(paymentBySecondID.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (paymentBySecondID.Value <= 0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User must be Null"
                };
                return faild;
            }
            if (!user.PaymentCode.Equals(paymentBySecondID.PaymentCode))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == user.Id);
            if (Wallet == null || Wallet.Total <= 0 || Wallet.Total < paymentBySecondID.Value)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "You Don't have enough balance in your Wallet"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var bus = await _db.Buses.Include(x => x.ApplicationDriver).FirstOrDefaultAsync(x => x.SocondID == paymentBySecondID.SecondID);
            if (bus == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "Check Bus ID"
                };
                return faild;
            }
            Guid Paymentid = Guid.NewGuid();
            var WPayment = new ApplicationWalletTrip
            {
                id = Paymentid,
                Payment_Date = localTime.DateTime,
                TripID = paymentBySecondID.TripID,
                UserID = user.Id,
                Value = paymentBySecondID.Value,
                BusID = bus.id,
                DriverID = bus.DriverID,
            };
            _db.WalletTrips.Add(WPayment);
            await _db.SaveChangesAsync();
            _db.Wallets.Attach(Wallet);
            Wallet.Total = Wallet.Total - paymentBySecondID.Value;
            await _db.SaveChangesAsync();
            if (bus.DriverID != null)
            {
                HttpClient client = new HttpClient();
                string url = "https://fcm.googleapis.com/fcm/send";
                var directRequest = new
                {
                    data = new
                    {
                        User = user.UserName,
                        Name = user.Name,
                        Bus = bus.PalteNumber,
                        Driver = bus.DriverID != null ? bus.ApplicationDriver.UserName : "No Driver Oo Bus",
                        Value = paymentBySecondID.Value,
                        status = true,
                        date = localTime.DateTime
                    },
                    to = bus.ApplicationDriver.FCMToken,
                    //to= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiUm91dGVTdGF0aW9uIiwiUm9sZSI6InN1cGVyQWRtaW4iLCJleHAiOjE2NDkzOTgxMDEsImlzcyI6IkludmVudG9yeUF1dGhlbnRpY2F0aW9uU2VydmVyIiwiYXVkIjoiSW52ZW50b3J5U2VydmljZVBvdG1hbkNsaWVudCJ9.TIiNpFfP3H6n1qfhR7cGi6WVxUDHhHgZE2wFx9HkNUc"

                };
                string key = "key=AAAAwVtOPVY:APA91bHSIWAQ15hFCs9T2lgNrHzUGWrLT_VzzlP6Ardo27z7_o3BMkTd542NBBaqvDye774pLx09SbsR5HxQGY5W3EnJJ24gnchAP5w4BOz0xb9eX-Zf4lR2vUB_ZQRn1nQ7yNFnLi2c";
                var directPaymentRequestJSON = JsonConvert.SerializeObject(directRequest);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", key);
                var httpContent = new StringContent(directPaymentRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                Console.WriteLine("SendNotfication");
            }

            var Seccess = new StatuseModel
            {
                Status = true,
                Message = Paymentid.ToString()
            };
            // await _hub.Clients.All.SendAsync("PaymentLive", "UserID:"+user.Id,"PaymentID:"+WPayment.id,"UserName:"+user.UserName,"Time:"+DateTime.UtcNow);
            return Seccess;
        }

        public async Task<StatuseModel> PaymentWallet(ApplicationPaymentWalletView paymentWalletView, ApplicationUser user)
        {
            if (!_api_key.Equals(paymentWalletView.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (!_api_secret.Equals(paymentWalletView.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (paymentWalletView.Value<=0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User must be Null"
                };
                return faild;
            }
            if (paymentWalletView.TripID == Guid.Empty)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Trip ID must be Null"
                };
                return faild;
            }
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == user.Id);
            if(Wallet==null || Wallet.Total<=0 || Wallet.Total < paymentWalletView.Value)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "You Don't have enough balance in your Wallet"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var bus = await _db.Buses.Include(x=>x.ApplicationDriver).FirstOrDefaultAsync(x=>x.id== paymentWalletView.BusID);
            if (bus == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "Check Bus ID"
                };
                return faild;
            }
            Guid Paymentid = Guid.NewGuid();
            var WPayment = new ApplicationWalletTrip
            {
                id= Paymentid,
                Payment_Date = localTime.DateTime,
                TripID = paymentWalletView.TripID,
                UserID = user.Id,
                Value = paymentWalletView.Value,
                BusID=paymentWalletView.BusID,
                DriverID=bus.DriverID,
            };
            _db.WalletTrips.Add(WPayment);
            _db.Wallets.Attach(Wallet);
            Wallet.Total = Wallet.Total - paymentWalletView.Value;
            await _db.SaveChangesAsync();
            Console.WriteLine("=====================================================");
            if (bus.DriverID != null)
            {
                HttpClient client = new HttpClient();
                string url = "https://fcm.googleapis.com/fcm/send";
                var directRequest = new
                {
                    data = new
                    {
                        User=user.UserName,
                        Name=user.Name,
                        Bus = bus.PalteNumber,
                        Driver = bus.DriverID!=null? bus.ApplicationDriver.UserName:"No Driver Oo Bus",
                        Value= paymentWalletView.Value,
                        status=true,
                        date=localTime.DateTime
                    },
                    to = bus.ApplicationDriver.FCMToken,
                    //to= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiUm91dGVTdGF0aW9uIiwiUm9sZSI6InN1cGVyQWRtaW4iLCJleHAiOjE2NDkzOTgxMDEsImlzcyI6IkludmVudG9yeUF1dGhlbnRpY2F0aW9uU2VydmVyIiwiYXVkIjoiSW52ZW50b3J5U2VydmljZVBvdG1hbkNsaWVudCJ9.TIiNpFfP3H6n1qfhR7cGi6WVxUDHhHgZE2wFx9HkNUc"

                };
                string key = "key=AAAAwVtOPVY:APA91bHSIWAQ15hFCs9T2lgNrHzUGWrLT_VzzlP6Ardo27z7_o3BMkTd542NBBaqvDye774pLx09SbsR5HxQGY5W3EnJJ24gnchAP5w4BOz0xb9eX-Zf4lR2vUB_ZQRn1nQ7yNFnLi2c";
                var directPaymentRequestJSON = JsonConvert.SerializeObject(directRequest);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", key);
                var httpContent = new StringContent(directPaymentRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                Console.WriteLine("SendNotfication");
            }
            
            var Seccess = new StatuseModel
            {
                Status = true,
                Message = Paymentid.ToString()
            };
           // await _hub.Clients.All.SendAsync("PaymentLive", "UserID:"+user.Id,"PaymentID:"+WPayment.id,"UserName:"+user.UserName,"Time:"+DateTime.UtcNow);
            return Seccess;

        }

        public async Task<StatuseModel> PaymentWalletByInspector(ApplicationPaymentWalletView paymentWalletView, ApplicationUser user)
        {
            if (!_api_key.Equals(paymentWalletView.api_key))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (!_api_secret.Equals(paymentWalletView.api_secret))
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "refused Payment"
                };
                return faild;
            }
            if (paymentWalletView.Value <= 0)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Value must be Grate Than 0"
                };
                return faild;
            }
            if (user == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User must be Null"
                };
                return faild;
            }
            if (paymentWalletView.TripID == Guid.Empty)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The Trip ID must be Null"
                };
                return faild;
            }
            var userPayment = await _manager.FindByIdAsync(paymentWalletView.UserID);
            if (userPayment == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "The User must be Null"
                };
                return faild;
            }
            var Wallet = await _db.Wallets.FirstOrDefaultAsync(x => x.UserID == userPayment.Id);
            if (Wallet == null || Wallet.Total <= 0 || Wallet.Total < paymentWalletView.Value)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "You Don't have enough balance in your Wallet"
                };
                return faild;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var bus = await _db.Buses.Include(x => x.ApplicationDriver).FirstOrDefaultAsync(x => x.id == paymentWalletView.BusID);
            if (bus == null)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = "Check Bus ID"
                };
                return faild;
            }
            Guid Paymentid = Guid.NewGuid();
            var WPayment = new ApplicationWalletTrip
            {
                id = Paymentid,
                Payment_Date = localTime.DateTime,
                TripID = paymentWalletView.TripID,
                UserID = userPayment.Id,
                Value = paymentWalletView.Value,
                BusID = paymentWalletView.BusID,
                DriverID = bus.DriverID,
                InspectorID=user.Id
            };
            _db.WalletTrips.Add(WPayment);
            _db.Wallets.Attach(Wallet);
            Wallet.Total = Wallet.Total - paymentWalletView.Value;
            await _db.SaveChangesAsync();
            Console.WriteLine("=====================================================");
            if (bus.DriverID != null)
            {
                HttpClient client = new HttpClient();
                string url = "https://fcm.googleapis.com/fcm/send";
                var directRequest = new
                {
                    data = new
                    {
                        User = userPayment.UserName,
                        Name = userPayment.Name,
                        Bus = bus.PalteNumber,
                        Driver = bus.DriverID != null ? bus.ApplicationDriver.UserName : "No Driver Oo Bus",
                        Value = paymentWalletView.Value,
                        status = true,
                        date = localTime.DateTime
                    },
                    to = bus.ApplicationDriver.FCMToken,
                    //to= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiUm91dGVTdGF0aW9uIiwiUm9sZSI6InN1cGVyQWRtaW4iLCJleHAiOjE2NDkzOTgxMDEsImlzcyI6IkludmVudG9yeUF1dGhlbnRpY2F0aW9uU2VydmVyIiwiYXVkIjoiSW52ZW50b3J5U2VydmljZVBvdG1hbkNsaWVudCJ9.TIiNpFfP3H6n1qfhR7cGi6WVxUDHhHgZE2wFx9HkNUc"

                };
                string key = "key=AAAAwVtOPVY:APA91bHSIWAQ15hFCs9T2lgNrHzUGWrLT_VzzlP6Ardo27z7_o3BMkTd542NBBaqvDye774pLx09SbsR5HxQGY5W3EnJJ24gnchAP5w4BOz0xb9eX-Zf4lR2vUB_ZQRn1nQ7yNFnLi2c";
                var directPaymentRequestJSON = JsonConvert.SerializeObject(directRequest);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", key);
                var httpContent = new StringContent(directPaymentRequestJSON, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                Console.WriteLine("SendNotfication");
            }

            var Seccess = new StatuseModel
            {
                Status = true,
                Message = Paymentid.ToString()
            };
            // await _hub.Clients.All.SendAsync("PaymentLive", "UserID:"+user.Id,"PaymentID:"+WPayment.id,"UserName:"+user.UserName,"Time:"+DateTime.UtcNow);
            return Seccess;
        }

        public async Task<double> SumChargeWallet()
        {
            var sum = await _db.WalletChargings.SumAsync(x => x.Value);
            return sum;
        }

        public async Task<double> SumPaymentWallet()
        {
            var sum = await _db.WalletTrips.SumAsync(x => x.Value);
            return sum;
        }

        public async Task<ApplicationWallet> Wallet(ApplicationUser user)
        {
            return await _db.Wallets.Include(x => x.User).FirstOrDefaultAsync(x => x.UserID == user.Id);
        }
    }
}

