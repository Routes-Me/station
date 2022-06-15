using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.ModelReturn;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Trip
{
	public class TripRep:ITripRep
	{
        private readonly ApplicationDb _db;

        public TripRep(ApplicationDb db)
		{
            _db = db;

        }

        public async Task<StatuseModel> Add(ApplicationTrip trip)
        {
            Guid id = Guid.NewGuid();
            trip.id = id;
            _db.Trips.Add(trip);
            await _db.SaveChangesAsync();
            var secess = new StatuseModel
            {
                Status = true,
                Message = id.ToString()
            };
            return secess;

        }

        public async Task<int> CountTrip()
        {
            return await _db.Trips.OrderBy(x => x.id).CountAsync();
        }

        public async Task<IEnumerable<ApplicationTrip>> List()
        {
            IQueryable<ApplicationTrip> queryable = _db.Set<ApplicationTrip>();
            queryable = queryable.Include(s => s.ApplicationRoute);
            queryable = queryable.Include(s => s.StartStation);
            queryable = queryable.Include(s => s.EndStation);
            queryable = queryable.Include(s => s.User);

            return await queryable.OrderBy(x => x.Creat_Date).ToListAsync();

        }

        public async Task<ApplicationGeneralListWithCountSum<ApplicationTrip>> ListByUser(ApplicationPagination pagination)
        {
            var Count = await _db.Trips.Where(x => x.UserID == pagination.id.ToString()).CountAsync();
            IQueryable<ApplicationTrip> queryable = _db.Set<ApplicationTrip>();
            queryable = queryable.Include(s => s.ApplicationRoute);
            queryable = queryable.Include(s => s.StartStation);
            queryable = queryable.Include(s => s.EndStation);
            queryable = queryable.Include(s => s.User);

            var Listret= await queryable.Where(x=>x.UserID==pagination.id.ToString()).OrderBy(x => x.Creat_Date).ToListAsync();
            return new ApplicationGeneralListWithCountSum<ApplicationTrip>
            {
                Count=Count,
                Sum=0,
                List=Listret
            };
        }
    }
}

