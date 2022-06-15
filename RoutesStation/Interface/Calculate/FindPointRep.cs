using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Calculate
{
	public class FindPointRep: IFindPointRep
	{
        private readonly ApplicationDb _db;
        private readonly ITowPointRep _towPointRep;

        public FindPointRep(ApplicationDb db, ITowPointRep towPointRep)
        {
            _db = db;
            _towPointRep = towPointRep;

        }

        

        public async Task<IEnumerable<ApplicationStationWithDistination>> FindPoint(ApplicationOnePointView pointView)
        {
            List<ApplicationStationWithDistination> ListStation = new List<ApplicationStationWithDistination>();
            var stations = await _db.RouteStationMaps.Include(x => x.ApplicationStation).Include(x => x.ApplicationRoute).Where(x => (x.ApplicationStation.Latitude <= pointView.Latitude + 0.02 && x.ApplicationStation.Latitude >= pointView.Latitude - 0.02) && (x.ApplicationStation.Longitude <= pointView.Longitude + 0.02 && x.ApplicationStation.Longitude >= pointView.Longitude - 0.02)).OrderByDescending(x => x.id).ToListAsync();

            foreach (var st in stations)
            {
                var points = new ApplicationTwoPointView
                {
                    Longitude1 = st.ApplicationStation.Longitude,
                    Latitude1 = st.ApplicationStation.Latitude,
                    Longitude2 = pointView.Longitude,
                    Latitude2 = pointView.Latitude
                };
                var calst = await _towPointRep.Dist(points);

                if (calst <= 0.75)
                {

                    var point = new ApplicationStationWithDistination
                    {
                        ApplicationRouteStationMap = st,
                        Distination = calst,
                    };
                    ListStation.Add(point);
                }
            }
            ListStation = ListStation.OrderBy(x => x.Distination).ToList();
            return ListStation;
        }

        

        public async Task<List<ApplicationRouteStationMap>> FindRoute(ApplicationRouteStationMap StartStation, ApplicationRouteStationMap EndStation)
        {
            List<ApplicationRouteStationMap> ListStation = new List<ApplicationRouteStationMap>();

            if (StartStation.RouteID == Guid.Empty)
            {
                return ListStation;
            }

            var rs = StartStation;
            var re = EndStation;

            IQueryable<ApplicationRouteStationMap> queryable = _db.Set<ApplicationRouteStationMap>();
            queryable = queryable.Include(s => s.ApplicationRoute);
            queryable = queryable.Include(s => s.ApplicationStation);

            if (rs == null || re == null)
            {
                return ListStation;
            }
            if (rs.Order < re.Order)
            {

                ListStation = await queryable.Where(x => x.RouteID == StartStation.RouteID && x.Order >= rs.Order && x.Order <= re.Order).OrderBy(x => x.Order).ToListAsync();
            }
            else
            {

                ListStation = await queryable.Where(x => x.RouteID == StartStation.RouteID && x.Order >= re.Order && x.Order <= rs.Order).OrderByDescending(x => x.Order).ToListAsync();
            }
            return ListStation;
        }

        

        public async Task<ApplicationStationMultiRoute> FindSharedPoint(IEnumerable<ApplicationStationWithDistination> point1, IEnumerable<ApplicationStationWithDistination> point2)
        {
            

            var rm = await _db.RouteStationMaps.Include(x=>x.ApplicationRoute).Include(x => x.ApplicationStation).ToListAsync();
            var shared1 = rm.Where(x => point1.Any(l => l.ApplicationRouteStationMap.RouteID == x.RouteID)).OrderBy(o=>o.Order);
            var shared2 = rm.Where(x => point2.Any(l => l.ApplicationRouteStationMap.RouteID == x.RouteID)).OrderBy(o => o.Order);
            //throw new NotImplementedException(JsonConvert.SerializeObject(shared1.Count()) + "=======" + JsonConvert.SerializeObject(shared2.Count()));
            var shared3 = shared1.FirstOrDefault(x => shared2.Any(y => y.StationID == x.StationID));
            var shared4 = shared2.FirstOrDefault(x => shared1.Any(y => y.StationID == x.StationID));
            var ListShared1 = point1.Where(x => x.ApplicationRouteStationMap.RouteID == shared3.RouteID);
            var ListShared2 = point2.Where(x => x.ApplicationRouteStationMap.RouteID == shared4.RouteID);
            
            List<ApplicationRouteStationMap> route1 = new List<ApplicationRouteStationMap>();
            List<ApplicationRouteStationMap> route2 = new List<ApplicationRouteStationMap>();
            
            if (ListShared1.ElementAt(0).ApplicationRouteStationMap.Order < shared3.Order)
            {

                /*StartP= ListShared1.ElementAt(1).ApplicationRouteStationMap;
              route1 = await FindRoute(ListShared1.ElementAt(1).ApplicationRouteStationMap, shared3);*/
                Console.WriteLine("==========================");

                var OP1 = new ApplicationOnePointView
                {
                    Latitude = shared3.ApplicationStation.Latitude,
                    Longitude = shared3.ApplicationStation.Longitude
                };
                var OP = new ApplicationOnePointView
                {
                    Latitude = ListShared1.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Latitude,
                    Longitude = ListShared1.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Longitude
                };
                var NewList1 = await FindPoint(OP);
               
                var NewList2 = await FindPoint(OP1);
               
                
                var t = NewList1.First(x => NewList2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID));
                var tt = NewList1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);
               
                var t1 = NewList2.FirstOrDefault(x => NewList1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID && y.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID
                && t.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction));
                
                var tt1 = NewList2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);


                if (t != null && t1 != null)
                {
                    if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order) && (tt.Count() > 2 && tt1.Count() > 2))
                    {
                        var r1 = tt.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction);
                        var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction && x.ApplicationRouteStationMap.RouteID == r1.ApplicationRouteStationMap.RouteID
                        && r1.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction);
                        route1 = await FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                    }
                    else
                    {
                        route1 = await FindRoute(t.ApplicationRouteStationMap, t1.ApplicationRouteStationMap);
                    }
                }
            }
            else
            {
                
                var OP1 = new ApplicationOnePointView
                {
                    Latitude = shared3.ApplicationStation.Latitude,
                    Longitude = shared3.ApplicationStation.Longitude
                };
                var OP = new ApplicationOnePointView
                {
                    Latitude = ListShared1.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Latitude,
                    Longitude = ListShared1.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Longitude
                };
                var NewList1 = await FindPoint(OP);
                var NewList2 = await FindPoint(OP1);
                
                var t = NewList1.First(x => NewList2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID));
                var tt = NewList1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);
                var t1 = NewList2.FirstOrDefault(x => NewList1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID && y.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID
                && t.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction));
                var tt1 = NewList2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);
                if (t != null && t1 != null)
                {
                    if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order) && (tt.Count() > 2 && tt1.Count() > 2))
                    {

                        var r1 = tt.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction);
                        var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction && x.ApplicationRouteStationMap.RouteID == r1.ApplicationRouteStationMap.RouteID
                        && r1.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction);
                        route1 = await FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                    }
                    else
                    {
                        route1 = await FindRoute(t.ApplicationRouteStationMap, t1.ApplicationRouteStationMap);

                    }
                }

            }

            if (shared4.Order > ListShared2.ElementAt(0).ApplicationRouteStationMap.Order)
            {
                
                
                var OP = new ApplicationOnePointView
                {
                    Latitude = shared4.ApplicationStation.Latitude,
                    Longitude = shared4.ApplicationStation.Longitude
                };
                var OP1 = new ApplicationOnePointView
                {
                    Latitude = ListShared2.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Latitude,
                    Longitude = ListShared2.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Longitude
                };
                var NewList1 = await FindPoint(OP);
                var NewList2 = await FindPoint(OP1);
                
                var t = NewList1.First(x => NewList2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID));
                var tt = NewList1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);
                var t1 = NewList2.FirstOrDefault(x => NewList1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID && y.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID
                && t.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction));
                var tt1 = NewList2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);
                if (t != null && t1 != null)
                {
                    if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order) && (tt.Count() > 2 && tt1.Count() > 2))
                    {
                        var r1 = tt.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction);
                        var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction && x.ApplicationRouteStationMap.RouteID == r1.ApplicationRouteStationMap.RouteID
                        && r1.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction);
                        route2 = await FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                    }
                    else
                    {
                        route2 = await FindRoute(t.ApplicationRouteStationMap, t1.ApplicationRouteStationMap);
                    }
                }
            }
            else
            {
                var OP = new ApplicationOnePointView
                {
                    Latitude = shared4.ApplicationStation.Latitude,
                    Longitude = shared4.ApplicationStation.Longitude
                };
                var OP1 = new ApplicationOnePointView
                {
                    Latitude = ListShared2.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Latitude,
                    Longitude = ListShared2.ElementAt(0).ApplicationRouteStationMap.ApplicationStation.Longitude
                };
                var NewList1 = await FindPoint(OP);
               
                var NewList2 = await FindPoint(OP1);
                
                var t = NewList1.First(x => NewList2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID));
                var tt = NewList1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);

                var t1 = NewList2.FirstOrDefault(x => NewList1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID && y.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID
                && t.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction));
                var tt1 = NewList2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);


                if (t != null && t1 != null)
                {
                    if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order) && (tt.Count() > 2 && tt1.Count() > 2))
                    {

                        var r1 = tt.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction);
                        var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction && x.ApplicationRouteStationMap.RouteID == r1.ApplicationRouteStationMap.RouteID
                        && r1.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction);
                        route2 = await FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                    }
                    else
                    {
                        route2 = await FindRoute(t.ApplicationRouteStationMap, t1.ApplicationRouteStationMap);
                    }
                }
            }

            


            var rout = new ApplicationStationMultiRoute
            {
                Rout1=route1,
                Rout2=route2,
                StartPoint=route1.FirstOrDefault(),
                SharedPoint1=route1.LastOrDefault(),
                SharedPoint2=route2.FirstOrDefault(),
                EndPoint=route2.LastOrDefault(),
            };
            return rout;
        }
    }
}


