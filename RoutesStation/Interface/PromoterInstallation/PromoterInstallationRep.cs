using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;

namespace RoutesStation.Interface.PromoterInstallation
{
	public class PromoterInstallationRep:IPromoterInstallationRep
	{
        private readonly ApplicationDb _db;

        public PromoterInstallationRep(ApplicationDb db)
		{
            _db = db;
        }

        public async Task<StatuseModel> Add(ApplicationPromoterInstallationMap station)
        {
            _db.PromoterInstallationMaps.Add(station);
            await _db.SaveChangesAsync();
            var secess = new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
            return secess;
        }

        public async Task<ApplicationPromoterInstallationMap> GetByUserID(string UserID)
        {
            return await _db.PromoterInstallationMaps.Include(x => x.ApplicationPromoter)
                .Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.UserID == UserID);
        }

        public async Task<IEnumerable<ApplicationPromoterInstallationMap>> List()
        {
            return await _db.PromoterInstallationMaps.Include(x => x.ApplicationPromoter)
                .Include(x => x.ApplicationUser).OrderBy(x => x.PromoterID).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationPromoterInstallationMap>> ListByPromoterID(string PromoterID)
        {
            return await _db.PromoterInstallationMaps.Include(x => x.ApplicationPromoter)
                .Include(x => x.ApplicationUser).Where(x => x.PromoterID == PromoterID).OrderBy(x => x.PromoterID).ToListAsync();
        }
    }
}

