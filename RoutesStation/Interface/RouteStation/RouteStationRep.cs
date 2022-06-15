using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.RouteStation
{
	public class RouteStationRep:IRouteStationRep
	{
        private readonly ApplicationDb _db;

        public RouteStationRep(ApplicationDb db)
		{
            _db = db;

        }

        public async Task<StatuseModel> Add(ApplicationRouteStationView routeStationMap)
        {
            try
            {
                var rs = await _db.RouteStationMaps.FirstOrDefaultAsync(x => x.StationID == routeStationMap.StationID && x.RouteID == routeStationMap.RouteID&& x.Direction==routeStationMap.Direction);
                if (rs != null)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Error this Station added To same Route before"
                    };
                    return faild;
                }
                var R = new ApplicationRouteStationMap
                {
                    Direction=routeStationMap.Direction,
                    Order=routeStationMap.Order,
                    RouteID=routeStationMap.RouteID,
                    StationID=routeStationMap.StationID,
                    HelpStation=routeStationMap.HelpStation,
                    Creat_Date=DateTime.UtcNow,
                };
                _db.RouteStationMaps.Add(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<StatuseModel> Delete(Guid id)
        {
            try
            {
                var R = await _db.RouteStationMaps.FindAsync(id);
                _db.RouteStationMaps.Remove(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<StatuseModel> Edit(ApplicationRouteStationView routeStationMap, Guid id)
        {
            try
            {
                var rs = await _db.RouteStationMaps.FirstOrDefaultAsync(x => x.StationID == routeStationMap.StationID && x.RouteID == routeStationMap.RouteID && x.Direction == routeStationMap.Direction);
                var rs1 = await _db.RouteStationMaps.FirstOrDefaultAsync(x => x.StationID == routeStationMap.StationID && x.RouteID == routeStationMap.RouteID && x.Order == routeStationMap.Order);
                if (rs != null&&rs.id!=id)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Error this Station added To same Route before in Same Diraction"
                    };
                    return faild;
                }
                if (rs1 != null&&rs1.id!=id)
                {
                    var faild = new StatuseModel
                    {
                        Status = false,
                        Message = "Error this Station added To same Route before in Same Order"
                    };
                    return faild;
                }
                var R = await _db.RouteStationMaps.FindAsync(id);
                _db.RouteStationMaps.Attach(R);

                R.Direction = routeStationMap.Direction;
                R.Order = routeStationMap.Order;
                R.RouteID = routeStationMap.RouteID;
                R.StationID = routeStationMap.StationID;
                R.HelpStation = routeStationMap.HelpStation;
                _db.Entry(R).Property(x => x.Direction).IsModified = true;
                _db.Entry(R).Property(x => x.Order).IsModified = true;
                _db.Entry(R).Property(x => x.RouteID).IsModified = true;
                _db.Entry(R).Property(x => x.StationID).IsModified = true;
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }

        public async Task<ApplicationRouteStationMap> Get(Guid id)
        {
            try
            {
                IQueryable<ApplicationRouteStationMap> queryable = _db.Set<ApplicationRouteStationMap>();
                queryable = queryable.Include(s => s.ApplicationRoute);
                queryable = queryable.Include(s => s.ApplicationStation);
                var R = await queryable.FirstOrDefaultAsync(x => x.id == id);

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new ApplicationRouteStationMap();
            }
        }

        public async Task<ApplicationRouteStationMap> GetLastRouteStation()
        {
            try
            {
                IQueryable<ApplicationRouteStationMap> queryable = _db.Set<ApplicationRouteStationMap>();
                queryable = queryable.Include(s => s.ApplicationRoute);
                queryable = queryable.Include(s => s.ApplicationStation);
                var R = await queryable.OrderBy(x=>x.Creat_Date).LastOrDefaultAsync();

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new ApplicationRouteStationMap();
            }
        }

        public async Task<ApplicationRouteStationMap> GetRouteStationByRouteID(Guid id)
        {
            try
            {
                IQueryable<ApplicationRouteStationMap> queryable = _db.Set<ApplicationRouteStationMap>();
                queryable = queryable.Include(s => s.ApplicationRoute);
                queryable = queryable.Include(s => s.ApplicationStation);
                var R = await queryable.OrderBy(x => x.Order).LastOrDefaultAsync(x=>x.RouteID==id);

                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new ApplicationRouteStationMap();
            }
        }

        public async Task<int> GetStationOrderInRoute(Guid RouteID)
        {
            try
            {

                var R = await _db.RouteStationMaps.OrderBy(x => x.Order).LastOrDefaultAsync(x => x.RouteID == RouteID);

                return R.Order;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<ApplicationRouteStationMap>> List()
        {
            try
            {
                IQueryable<ApplicationRouteStationMap> queryable = _db.Set<ApplicationRouteStationMap>();
                queryable = queryable.Include(s => s.ApplicationRoute);
                queryable = queryable.Include(s => s.ApplicationStation);
                var R = await queryable.OrderBy(x=>x.RouteID).ToListAsync();
                 
                return R;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new List<ApplicationRouteStationMap>();
            }
        }
        public async Task<int> CountRouteStation()
        {
            return await _db.RouteStationMaps.OrderBy(x => x.id).CountAsync();
        }

        public async Task<bool> ReOrderInRoute(Guid RouteID)
        {
            var ListRoute = await _db.RouteStationMaps.Where(x => x.RouteID == RouteID).OrderBy(x=>x.Order).ToListAsync();
            _db.RouteStationMaps.AttachRange(ListRoute);
            int i =1;
            foreach(var item in ListRoute)
            {
                //var rs = await _db.RouteStationMaps.FindAsync(item.id);
                item.Order = i;
                i++;
                
            }
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<StatuseModel> DeleteRouteStationByRoute(Guid RouteID)
        {
            try
            {
                var R = await _db.RouteStationMaps.Where(x=>x.RouteID==RouteID).ToListAsync();
                _db.RouteStationMaps.RemoveRange(R);
                await _db.SaveChangesAsync();
                var secess = new StatuseModel
                {
                    Status = true,
                    Message = "Seccess"
                };
                return secess;
            }
            catch (Exception ex)
            {
                var faild = new StatuseModel
                {
                    Status = false,
                    Message = ex.ToString()
                };
                return faild;
            }
        }
    }
}

