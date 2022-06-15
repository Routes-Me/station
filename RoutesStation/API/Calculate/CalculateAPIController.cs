using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Calculate;
using RoutesStation.Models;
using RoutesStation.ModelsView;
using System.Linq;
using System.Net;

namespace RoutesStation.API.Calculate
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CalculateAPIController : ControllerBase
    {
        private readonly IFindPointRep _find;

        public CalculateAPIController(IFindPointRep find)
        {
            _find = find;
        }
        
        [HttpPost]
        [Route("FindRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> FindRoute1(ApplicationTwoPointView point)
        {

            try
            {
                var point1 = new ApplicationOnePointView()
                {
                    Longitude = point.Longitude1,
                    Latitude = point.Latitude1
                };
                var point2 = new ApplicationOnePointView()
                {
                    Longitude = point.Longitude2,
                    Latitude = point.Latitude2
                };
                var List1 = await _find.FindPoint(point1);
                var List2 = await _find.FindPoint(point2);

                var t = List1.First(x => List2.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID));
                var tt = List1.Where(x => x.ApplicationRouteStationMap.RouteID == t.ApplicationRouteStationMap.RouteID);
                
                var t1 = List2.FirstOrDefault(x => List1.Any(y => y.ApplicationRouteStationMap.RouteID == x.ApplicationRouteStationMap.RouteID&&y.ApplicationRouteStationMap.RouteID==t.ApplicationRouteStationMap.RouteID
                &&t.ApplicationRouteStationMap.Direction==x.ApplicationRouteStationMap.Direction));
                var tt1= List2.Where(x => x.ApplicationRouteStationMap.RouteID == t1.ApplicationRouteStationMap.RouteID);
                /*return Ok(new
                {
                    o1 = tt.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        RouteName=x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        StationName=x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Direction = x.ApplicationRouteStationMap.Direction,
                        Dist=x.Distination
                    }),
                    o2= tt1.Select(x => new
                    {
                        Order = x.ApplicationRouteStationMap.Order,
                        RouteName = x.ApplicationRouteStationMap.ApplicationRoute.Name_EN,
                        StationName = x.ApplicationRouteStationMap.ApplicationStation.Title_EN,
                        Direction = x.ApplicationRouteStationMap.Direction,
                        Dist = x.Distination
                    })
                });*/
                    var res = new List<ApplicationRouteStationMap>();
                 var x = 0;
                 if (t != null && t1 != null)
                 {

                    if ((t.ApplicationRouteStationMap.Order > t1.ApplicationRouteStationMap.Order)&&(tt.Count()>1&&tt1.Count()>1))
                    {
                        
                        var r1 = tt.FirstOrDefault(x=>x.ApplicationRouteStationMap.Direction!=t1.ApplicationRouteStationMap.Direction);
                        var r2 = tt1.FirstOrDefault(x => x.ApplicationRouteStationMap.Direction != t1.ApplicationRouteStationMap.Direction&&x.ApplicationRouteStationMap.RouteID==r1.ApplicationRouteStationMap.RouteID
                        && r1.ApplicationRouteStationMap.Direction == x.ApplicationRouteStationMap.Direction);
                        res = await _find.FindRoute(r1.ApplicationRouteStationMap, r2.ApplicationRouteStationMap);
                        if (res != null) return Ok(new
                        {
                            status = true,

                            description = new
                            {
                                StartPoint = new { Longitude = point.Longitude1, Latitude = point.Latitude1 },
                                StartStation = new { id = r1.ApplicationRouteStationMap.StationID, Title = r1.ApplicationRouteStationMap.ApplicationStation.Title_EN,Order=r1.ApplicationRouteStationMap.Order, Longitude = r1.ApplicationRouteStationMap.ApplicationStation.Longitude, Latitude = r1.ApplicationRouteStationMap.ApplicationStation.Latitude },
                                DestinationFromPointToStationStart = r1.Distination,
                                EndPoint = new { Longitude = point.Longitude2, Latitude = point.Latitude2 },
                                EndStation = new { id = r2.ApplicationRouteStationMap.StationID, Title = r2.ApplicationRouteStationMap.ApplicationStation.Title_EN, Order = r2.ApplicationRouteStationMap.Order, Longitude = r2.ApplicationRouteStationMap.ApplicationStation.Longitude, Latitude = r2.ApplicationRouteStationMap.ApplicationStation.Latitude },
                                DestinationFromPointToStationEnd = r2.Distination,
                                FromTo = t1.ApplicationRouteStationMap.ApplicationRoute.From_To_EN,
                                res = res.Select(x => new
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
                            }
                        });

                    }
                    else
                    {
                        
                        res = await _find.FindRoute(t.ApplicationRouteStationMap, t1.ApplicationRouteStationMap);

                    }



                }

                 if (res != null) return Ok(new
                 {
                     status = true,

                     description = new
                     {
                         StartPoint = new { Longitude = point.Longitude1, Latitude = point.Latitude1 },
                         StartStation = new { id = t.ApplicationRouteStationMap.StationID, Title = t.ApplicationRouteStationMap.ApplicationStation.Title_EN, Order = t.ApplicationRouteStationMap.Order,Route=t.ApplicationRouteStationMap.ApplicationRoute.Name_EN, Longitude = t.ApplicationRouteStationMap.ApplicationStation.Longitude, Latitude = t.ApplicationRouteStationMap.ApplicationStation.Latitude },
                         DestinationFromPointToStationStart = t.Distination,
                         EndPoint = new { Longitude = point.Longitude2, Latitude = point.Latitude2 },
                         EndStation = new { id = t1.ApplicationRouteStationMap.StationID, Title = t1.ApplicationRouteStationMap.ApplicationStation.Title_EN, Order = t1.ApplicationRouteStationMap.Order, Route = t1.ApplicationRouteStationMap.ApplicationRoute.Name_EN, Longitude = t1.ApplicationRouteStationMap.ApplicationStation.Longitude, Latitude = t1.ApplicationRouteStationMap.ApplicationStation.Latitude },
                         DestinationFromPointToStationEnd = t1.Distination,
                         FromTo = t1.ApplicationRouteStationMap.ApplicationRoute.From_To_EN,
                         res = res.Select(x => new
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
                             DiractionRoute=x.Direction,
                             directionStation = x.ApplicationStation.DirectionStation
                         })
                     }
                 });
                 return Ok(new
                 {
                     status = false,
                     description = "Dont have any point"
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

    