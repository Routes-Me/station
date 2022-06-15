using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Interface.Bus;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Driver
{
	public class DriverRep:IDriverRep
	{
        private readonly ApplicationDb _db;

        public DriverRep(ApplicationDb db)
		{
            _db = db;

        }

        public async Task<StatuseModel> Delete(ApplicationUser user)
        {
            var data = await _db.Users.FindAsync(user.Id);
            if (data!=null)
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "This is not Driver"

                };
                return Falid;
            }
            _db.Users.Remove(data);
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<StatuseModel> Enter(ApplicationBusDriveView model, ApplicationUser user)
        {
            var bus = await _db.Buses.FindAsync(model.BusID);
            
            if (bus.Active)
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "This Bus Useing from another Driver1"

                };
                return Falid;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);

            var company = await _db.Buses.OrderBy(x=>x.id).FirstOrDefaultAsync(x => x.id == model.BusID);
            var data = new ApplicationBusDriverMap
            {
                BusID = model.BusID,
                DriverID = user.Id,
                Start_Date = localTime.DateTime,
            };
            _db.BusDriverMaps.Add(data);
            _db.Buses.Attach(bus);
            bus.Active = true;
            bus.DriverID = user.Id;
            _db.Users.Attach(user);
            user.FCMToken = model.FCMToken;
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = data.id.ToString()
                
            };
            return seccess;

        }

        public async Task<StatuseModel> Out(ApplicationBusDriveView model, ApplicationUser user)
        {
            var bus = await _db.Buses.FindAsync(model.BusID);
            if (!bus.Active)
            {
                var Falid = new StatuseModel
                {
                    Status = true,
                    Message = "This Bus not Useing"

                };
                return Falid;
            }
            var info = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            var data = await _db.BusDriverMaps.OrderBy(x => x.Start_Date).LastOrDefaultAsync(x => x.BusID == model.BusID && x.DriverID == user.Id);
            _db.BusDriverMaps.Attach(data);
            data.End_Date = localTime.DateTime;
            _db.Buses.Attach(bus);
            bus.Active = false;
            bus.DriverID = null;
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;

        }
    }
}

