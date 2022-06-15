using System;
using Microsoft.EntityFrameworkCore;
using RoutesStation.ModelReturn;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Route
{
	public class RouteRequestRep:IRouteRequestRep
	{
        private readonly ApplicationDb _db;

        public RouteRequestRep(ApplicationDb db)
		{
            _db = db;

        }

        public async Task<StatuseModel> Add(ApplicationRouteRequest route)
        {
            _db.RouteRequests.Add(route);
            await _db.SaveChangesAsync();
            return new StatuseModel
            {
                Status=true,
                Message="Seccess"
            };
        }

        public async Task<StatuseModel> Delete(Guid id)
        {
            var route = await _db.RouteRequests.FindAsync(id);
            _db.RouteRequests.Remove(route);
            await _db.SaveChangesAsync();
            return new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
        }

        public async Task<StatuseModel> DeleteRange(List<Guid> id)
        {
            _db.RouteRequests.RemoveRange(id.Select(i => new ApplicationRouteRequest { id = i }));
            await _db.SaveChangesAsync();
            return new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };

        }

        public async Task<StatuseModel> Edit(ApplicationRouteRequest route)
        {
            var data = await _db.RouteRequests.FindAsync(route.id);
            data = route;
            await _db.SaveChangesAsync();
            return new StatuseModel
            {
                Status = true,
                Message = "Seccess"
            };
        }

        public async Task<ApplicationRouteRequest> Get(Guid id)
        {
            return await _db.RouteRequests.Include(x => x.ApplicationAdmin).Include(x => x.ApplicationCompany).FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<ApplicationGeneralListWithCountSum<ApplicationRouteRequest>> List(ApplicationPagination pagination)
        {
            var Count = await _db.RouteRequests.CountAsync();
            var Listret=  await _db.RouteRequests.Include(x => x.ApplicationAdmin).Include(x => x.ApplicationCompany).OrderByDescending(x =>x.Read).ThenByDescending(x=>x.Request_Date)
                .Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToListAsync();
            return new ApplicationGeneralListWithCountSum<ApplicationRouteRequest>
            {
                Count=Count,
                List=Listret
            };
        }
    }
}

