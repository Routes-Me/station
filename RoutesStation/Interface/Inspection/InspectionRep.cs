using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;

namespace RoutesStation.Interface.Inspection
{
	public class InspectionRep:IInspectionRep
	{
        private readonly ApplicationDb _db;

        public InspectionRep(ApplicationDb db)
		{
            _db = db;

        }

        public async Task<StatuseModel> AddInspectionBus(ApplicationInspectionBusMap inspectionBusMap)
        {
            _db.InspectionBusMaps.Add(inspectionBusMap);
            await _db.SaveChangesAsync();
            var secess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
            return secess;
        }

        public async Task<StatuseModel> AddInspectionUser(ApplicationInspectionUserMap inspectionUserMap)
        {
            _db.InspectionUserMaps.Add(inspectionUserMap);
            await _db.SaveChangesAsync();
            var secess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
            return secess;
        }

        public async Task<IEnumerable<ApplicationInspectionBusMap>> InspectionBusList()
        {
            return await _db.InspectionBusMaps.Include(x => x.ApplicationBus).ThenInclude(x=>x.ApplicationCompany)
                .Include(x => x.ApplicationBus).ThenInclude(x => x.ApplicationRoute)
                .Include(x => x.ApplicationInspector)
                .OrderByDescending(x => x.Creat_Date).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationInspectionUserMap>> InspectionUserList()
        {
            return await _db.InspectionUserMaps.Include(x => x.ApplicationUser)
                .Include(x => x.ApplicationInspector)
                .OrderByDescending(x => x.Creat_Date).ToListAsync();
        }
    }
}

