using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutesStation.Interface.Calculate;
using RoutesStation.Models;
using RoutesStation.ModelsView;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace RoutesStation.API.Calculate
{
    [Route("api/test")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class CalculateMultiTestAPIController : ControllerBase
    {
        private readonly IFindPointRep _find;
        private readonly ApplicationDb _db;
        private readonly ITowPointRep _towPointRep;

        public CalculateMultiTestAPIController(IFindPointRep find,ApplicationDb db, ITowPointRep towPointRep)
        {
            _find = find;
            _db = db;
            _towPointRep = towPointRep;
        }
        [HttpPost]
        [Route("FindMultiRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> FindRoute(ApplicationTwoPointView point)
        {
            List<ApplicationRouteStationMap> route1 = new List<ApplicationRouteStationMap>();
            List<ApplicationRouteStationMap> route2 = new List<ApplicationRouteStationMap>();
            try
            {
                var point11 = new ApplicationOnePointView()
                {
                    Longitude = point.Longitude1,
                    Latitude = point.Latitude1
                };
                var point22 = new ApplicationOnePointView()
                {
                    Longitude = point.Longitude2,
                    Latitude = point.Latitude2
                };
                var List1 = await _find.FindPoint(point11);
                var List2 = await _find.FindPoint(point22);

                //FindSharedPoint

                if (List1.Count() == 0 || List2.Count() == 0)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "NO Point"
                    });
                }

                var ListRoute1 = await _db.RouteStationMaps.Include(x => x.ApplicationStation).Include(x=>x.ApplicationRoute).Where(x => x.ApplicationStation.Latitude == List1.First().ApplicationRouteStationMap.ApplicationStation.Latitude&& x.ApplicationStation.Longitude == List1.First().ApplicationRouteStationMap.ApplicationStation.Longitude).ToListAsync();
                var ListRoute2 = await _db.RouteStationMaps.Include(x => x.ApplicationStation).Include(x => x.ApplicationRoute).Where(x => x.ApplicationStation.Latitude == List2.First().ApplicationRouteStationMap.ApplicationStation.Latitude && x.ApplicationStation.Longitude == List2.First().ApplicationRouteStationMap.ApplicationStation.Longitude).ToListAsync();
                var ListSharedStation1 = await _db.RouteStationMaps.Include(x => x.ApplicationStation).Include(x => x.ApplicationRoute).Where(x => ListRoute1.First().RouteID == x.RouteID).ToListAsync();
                var ListSharedStation2 = await _db.RouteStationMaps.Include(x => x.ApplicationStation).Include(x => x.ApplicationRoute).Where(x => ListRoute2.First().RouteID == x.RouteID).ToListAsync();
                var sharedStationRoute1withRout2 = ListSharedStation1.Where(x => ListSharedStation2.Any(y => y.StationID == x.StationID)).OrderBy(x=>x.Order).ToList();
                var sharedStationRoute2withRout1 = ListSharedStation2.Where(x => ListSharedStation1.Any(y => y.StationID == x.StationID)).OrderBy(x => x.Order).ToList();
                var startPoint = ListRoute1.FirstOrDefault(x => sharedStationRoute1withRout2.Any(y => y.RouteID == x.RouteID));
                var endPoint = ListRoute2.FirstOrDefault(x => sharedStationRoute2withRout1.Any(y => y.RouteID == x.RouteID));
               
                    Console.WriteLine("===========111");
                Console.WriteLine(sharedStationRoute1withRout2.Count());
                Console.WriteLine("===========222");
                List<ApplicationStationWithDistination> Listshared1withDistination = new List<ApplicationStationWithDistination>();
                List<ApplicationStationWithTowDistination> Listshared2withDistination = new List<ApplicationStationWithTowDistination>();
                foreach (var item in sharedStationRoute1withRout2)
                {
                    Console.WriteLine("===========");
                    Console.WriteLine(item.Order);
                    var dist = item.Order - startPoint.Order;
                    if (dist < 0)
                    {
                        dist = dist * -1;
                    }
                    var item1 = new ApplicationStationWithDistination
                    {
                        ApplicationRouteStationMap = item,
                        Distination = dist
                    };
                    Listshared1withDistination.Add(item1);
                }
                Listshared1withDistination = Listshared1withDistination.OrderBy(x => x.Distination).ToList();
                foreach (var item in sharedStationRoute2withRout1)
                {
                    var dist = item.Order - endPoint.Order;
                    if (dist < 0)
                    {
                        dist = dist * -1;
                    }
                    var points = new ApplicationTwoPointView
                    {
                        Latitude1=item.ApplicationStation.Latitude,
                        Longitude1=item.ApplicationStation.Longitude,
                        Latitude2= Listshared1withDistination.First().ApplicationRouteStationMap.ApplicationStation.Latitude,
                        Longitude2= Listshared1withDistination.First().ApplicationRouteStationMap.ApplicationStation.Longitude,

                    };
                    var diststart =await _towPointRep.Dist(points);
                    var item1 = new ApplicationStationWithTowDistination
                    {
                        ApplicationRouteStationMap = item,
                        DistinationEnd = dist,
                        DistinationStart= diststart,

                    };
                    Listshared2withDistination.Add(item1);
                }
                
                Listshared2withDistination= Listshared2withDistination.OrderBy(x => x.DistinationStart).ThenBy(x=>x.DistinationEnd).ToList();
                /*return StatusCode(200, new
                {
                    o1 = Listshared1withDistination.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        Route = x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        Station = x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Distination = x.Distination
                    }),
                    o2 = Listshared2withDistination.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        Route = x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        Station = x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Distination1 = x.DistinationStart,
                        Distination2 = x.DistinationEnd
                    })

                });*/

                if (startPoint.Order< Listshared1withDistination.First().ApplicationRouteStationMap.Order)
                {
                    route1 = await _find.FindRoute(startPoint, Listshared1withDistination.First().ApplicationRouteStationMap);
                }
                else
                {
                    var OP = new ApplicationOnePointView
                    {
                        Latitude = startPoint.ApplicationStation.Latitude,
                        Longitude = startPoint.ApplicationStation.Longitude
                    };
                    var OP1 = new ApplicationOnePointView
                    {
                        Latitude = Listshared1withDistination.First().ApplicationRouteStationMap.ApplicationStation.Latitude,
                        Longitude = Listshared1withDistination.First().ApplicationRouteStationMap.ApplicationStation.Longitude
                    };
                    var NewList1 = await _find.FindPoint(OP);
                    var NewList2 = await _find.FindPoint(OP1);

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
                            route1 = await _find.FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                        }
                        else
                        {
                            return StatusCode(200, new
                            {
                                status = false,
                                description = "No Route"
                            });

                        }
                    }
                }
               
                if (Listshared2withDistination.First().ApplicationRouteStationMap.Order < endPoint.Order)
                {
                    
                    route2 = await _find.FindRoute(Listshared2withDistination.First().ApplicationRouteStationMap, endPoint);
                }
                else
                {
                    
                    var OP1 = new ApplicationOnePointView
                    {
                        Latitude = endPoint.ApplicationStation.Latitude,
                        Longitude = endPoint.ApplicationStation.Longitude
                    };
                    var OP = new ApplicationOnePointView
                    {
                        Latitude = Listshared2withDistination.First().ApplicationRouteStationMap.ApplicationStation.Latitude,
                        Longitude = Listshared2withDistination.First().ApplicationRouteStationMap.ApplicationStation.Longitude
                    };
                    var NewList1 = await _find.FindPoint(OP);
                    var NewList2 = await _find.FindPoint(OP1);

                    

                    var t = NewList1.First(x => NewList2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID&&x.ApplicationRouteStationMap.RouteID==endPoint.RouteID));
                    var tt = NewList1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);
                    var t1 = NewList2.FirstOrDefault(x => NewList1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID && y.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID
                    && t.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction && x.ApplicationRouteStationMap.RouteID == endPoint.RouteID));
                    var tt1 = NewList2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);

                    

                    if (t != null && t1 != null)
                    {
                        if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order) && (tt.Count() > 1 && tt1.Count() > 1))
                        {

                            var r1 = tt.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction);
                            var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.RouteID == r1.ApplicationRouteStationMap.RouteID);

                            route2 = await _find.FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                        }
                        else
                        {

                            return StatusCode(200, new
                            {
                                status = false,
                                description = "No Route"
                            });
                        }
                    }
                }
                

                var result = new ApplicationStationMultiRoute
                {
                    Rout1 = route1,
                    Rout2 = route2,
                    StartPoint = route1.FirstOrDefault(),
                    SharedPoint1 = route1.LastOrDefault(),
                    SharedPoint2 = route2.FirstOrDefault(),
                    EndPoint = route2.LastOrDefault(),
                };
                //END FindSharedPoint
              

                //var result = await _find.FindSharedPoint(List1, List2);
                return StatusCode(200, new
                {
                    status = true,
                    StartPoint = new { Latitude = point.Latitude1, Longitude = point.Longitude1 },
                    StartStation = new { id = result.StartPoint.StationID, Title = result.StartPoint.ApplicationStation.Title_EN, Rout = result.StartPoint.ApplicationRoute.Name_EN, Order = result.StartPoint.Order, Longitude = result.StartPoint.ApplicationStation.Longitude, Latitude = result.StartPoint.ApplicationStation.Latitude },

                    SharedPoint1 = new { id = result.SharedPoint1.StationID, Title = result.SharedPoint1.ApplicationStation.Title_EN, Rout = result.StartPoint.ApplicationRoute.Name_EN, Order = result.SharedPoint1.Order, Longitude = result.SharedPoint1.ApplicationStation.Longitude, Latitude = result.SharedPoint1.ApplicationStation.Latitude },
                    SharedPoint2 = new { id = result.SharedPoint2.StationID, Title = result.SharedPoint2.ApplicationStation.Title_EN, Rout = result.EndPoint.ApplicationRoute.Name_EN, Order = result.SharedPoint2.Order, Longitude = result.SharedPoint2.ApplicationStation.Longitude, Latitude = result.SharedPoint2.ApplicationStation.Latitude },
                    EndPoint = new { Latitude = point.Latitude2, Longitude = point.Longitude2 },
                    EndStation = new { id = result.EndPoint.StationID, Title = result.EndPoint.ApplicationStation.Title_EN, Rout = result.EndPoint.ApplicationRoute.Name_EN, Order = result.EndPoint.Order, Longitude = result.EndPoint.ApplicationStation.Longitude, Latitude = result.EndPoint.ApplicationStation.Latitude },

                    FromToRout1 = result.StartPoint.ApplicationRoute.From_To_EN,
                    FromToRout2 = result.EndPoint.ApplicationRoute.From_To_EN,
                    rout1 = result.Rout1.Select(x => new
                    {
                        id = x.id,
                        order = x.Order,
                        direction = x.Direction,
                        helpStation = x.HelpStation,
                        routeID = x.RouteID,
                        route = x.ApplicationRoute.Name_EN,
                        price = x.ApplicationRoute.Price,
                        StationId = x.StationID,
                        station = x.ApplicationStation.Title_EN,
                        longitude = x.ApplicationStation.Longitude,
                        latitude = x.ApplicationStation.Latitude,
                        DiractionRoute = x.Direction,
                        directionStation = x.ApplicationStation.DirectionStation
                    }),
                    rout2 = result.Rout2.Select(x => new
                    {
                        id = x.id,
                        order = x.Order,
                        direction = x.Direction,
                        helpStation = x.HelpStation,
                        routeID = x.RouteID,
                        route = x.ApplicationRoute.Name_EN,
                        price = x.ApplicationRoute.Price,
                        StationId = x.StationID,
                        station = x.ApplicationStation.Title_EN,
                        longitude = x.ApplicationStation.Longitude,
                        latitude = x.ApplicationStation.Latitude,
                        DiractionRoute = x.Direction,
                        directionStation = x.ApplicationStation.DirectionStation
                    })
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


        [Route("Far")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Far(ApplicationTwoPointView point)
        {
            


            try
            {
                var calst = await _towPointRep.Dist(point);

                return StatusCode(200, new
                {
                    status = false,
                    description = calst
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

        [Route("Station1")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Station1(ApplicationTwoPointView point)
        {
            try
            {
                var t = await _db.Stations.Where(x => x.Latitude.ToString().Contains(point.Latitude1.ToString()) && x.Longitude.ToString().Contains(point.Longitude1.ToString())).ToListAsync();

                return StatusCode(200, new
                {
                    status = false,
                    description = t
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

        [Route("Route1")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Route1(ApplicationRequestId applicationRequestId)
        {
            try
            {
                var t = await _db.RouteStationMaps.Include(x=>x.ApplicationRoute).Where(x=>x.StationID==applicationRequestId.id).ToListAsync();

                return StatusCode(200, new
                {
                    status = false,
                    description = t
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

    }


  }
/*
 * return StatusCode(200, new
                {
                    a= Listshared1withDistination.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        Route=x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        Station=x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Dist=x.Distination

                    }),
                    b = Listshared2withDistination.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        Route = x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        Station = x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Dist = x.DistinationStart,
                        Diste=x.DistinationEnd

                    })

                });*/
    