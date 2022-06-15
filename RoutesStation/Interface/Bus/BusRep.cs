using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Bus
{
	public class BusRep:IBusRep
	{
        private readonly ApplicationDb _db;
        private readonly string _api_key;
        private readonly string _api_secret;

        public BusRep(ApplicationDb db)
		{
            _db = db;
            _api_key = "$FhlF]3;.OIic&{>H;_DeW}|:wQ,A8";
            _api_secret = "Z~P7-_/i!=}?BIwAd*S67LBzUo4O^G";

        }

        public async Task<StatuseModel> ActiveBus(ApplicationSecondID SecondID)
        {
            if (!SecondID.api_key.Equals(_api_key)||!SecondID.api_secret.Equals(_api_secret))
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "Refused Operation"

                };
                return Falid;
            }
            var bus = await _db.Buses.FirstOrDefaultAsync(x=>x.SocondID==SecondID.SecondID);
            if (bus==null)
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "Check Bus SecondID"

                };
                return Falid;
            }
            if (bus.Active)
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "This Bus Useing from another Driver1"

                };
                return Falid;
            }
            _db.Buses.Attach(bus);
            bus.Active = true;
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = bus.id.ToString()

            };
            return seccess;
        }

        public async Task<StatuseModel> Add(ApplicationBus model)
        {
            _db.Buses.Add(model);
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<StatuseModel> Delete(Guid id)
        {
            var data = await _db.Buses.FindAsync(id);
            _db.Buses.Remove(data);
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;

        }

        public async Task<StatuseModel> Edite(ApplicationBus model)
        {
            var data = await _db.Buses.FindAsync(model.id);
            _db.Buses.Attach(data);
            data = model;
            await _db.SaveChangesAsync();
            var seccess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"

            };
            return seccess;
        }

        public async Task<ApplicationBus> Get(Guid id)
        {
            return await _db.Buses.Include(x => x.ApplicationRoute)
                .Include(x=>x.ApplicationDriver).Include(x=>x.ApplicationCompany).OrderBy(x => x.CompanyID).FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<IEnumerable<ApplicationBus>> List()
        {
            return await _db.Buses.Include(x => x.ApplicationRoute).Include(x=>x.ApplicationDriver)
                .Include(x=>x.ApplicationCompany).OrderBy(x => x.CompanyID).ToListAsync();
        }

        public async Task<StatuseModel> UnActiveBus(ApplicationSecondID SecondID)
        {
            if (!SecondID.api_key.Equals(_api_key) || !SecondID.api_secret.Equals(_api_secret))
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "Refused Operation"

                };
                return Falid;
            }
            var bus = await _db.Buses.FirstOrDefaultAsync(x => x.SocondID == SecondID.SecondID);
            if (!bus.Active)
            {
                var Falid = new StatuseModel
                {
                    Status = false,
                    Message = "This Bus alredy UnActive"

                };
                return Falid;
            }
            _db.Buses.Attach(bus);
            bus.Active = false;
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

