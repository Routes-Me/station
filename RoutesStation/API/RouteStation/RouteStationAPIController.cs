using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.RouteStation;
using RoutesStation.Models;
using RoutesStation.ModelsView;
using System.Net;

namespace RoutesStation.API
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class RouteStationAPIController : ControllerBase
    {
        private readonly IRouteStationRep _routeStation;

        public RouteStationAPIController(IRouteStationRep routeStation)
        {
            _routeStation = routeStation;
        }
        [HttpPost]
        [Route("AddRouteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationRouteStationView routestation)
        {

          try
            {
                var result = await _routeStation.Add(routestation);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Message
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
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
        [HttpPost]
        [Route("EditRouteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationRouteStationView routestation)
        {

         try
            {
                var result = await _routeStation.Edit(routestation, routestation.id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Message
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
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
        [HttpPost]
        [Route("DeleteRouteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(ApplicationRequestId id)
        {

         try
            {
                var result = await _routeStation.Delete(id.id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Message
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
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

        [HttpPost]
        [Route("GetRouteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {

         try
            {
                var result = await _routeStation.Get(id.id);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id=result.id,
                            RouteId=result.RouteID,
                            Name_Route_AR = result.ApplicationRoute.Name_AR,
                            Name_Route_EN = result.ApplicationRoute.Name_EN,
                            Area_AR = result.ApplicationRoute.Area_AR,
                            Area_EN = result.ApplicationRoute.Name_EN,
                            StationId=result.StationID,
                            Title_Station_AR=result.ApplicationStation.Title_AR,
                            Title_Station_EN = result.ApplicationStation.Title_EN,
                            Direction=result.Direction,
                            Order=result.Order,
                            HelpStation=result.HelpStation,
                        },
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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

        [HttpPost]
        [Route("ReorderRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> ReorderRoute(ApplicationRequestId id)
        {

            try
            {
                var result = await _routeStation.ReOrderInRoute(id.id);

                if (result)
                {
                    return Ok(new
                    {
                        status = true,
                        description = "Seccess"
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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

        [HttpGet]
        [Route("GetLastRouteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> GetLastRouteStation()
        {
            try
            {
                var result = await _routeStation.GetLastRouteStation();

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id = result.id,
                            RouteId = result.RouteID,
                            Name_Route_AR = result.ApplicationRoute.Name_AR,
                            Name_Route_EN = result.ApplicationRoute.Name_EN,
                            Area_AR = result.ApplicationRoute.Area_AR,
                            Area_EN = result.ApplicationRoute.Name_EN,
                            StationId = result.StationID,
                            Title_Station_AR = result.ApplicationStation.Title_AR,
                            Title_Station_EN = result.ApplicationStation.Title_EN,
                            Direction = result.Direction,
                            Order = result.Order,
                            HelpStation = result.HelpStation,
                        },
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result
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


        [HttpPost]
        [Route("GetRouteStationByRouteID")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRouteStationByRouteID(ApplicationRequestId id)
        {
            try
            {
                var result = await _routeStation.GetRouteStationByRouteID(id.id);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id = result.id,
                            RouteId = result.RouteID,
                            Name_Route_AR = result.ApplicationRoute.Name_AR,
                            Name_Route_EN = result.ApplicationRoute.Name_EN,
                            Area_AR = result.ApplicationRoute.Area_AR,
                            Area_EN = result.ApplicationRoute.Name_EN,
                            StationId = result.StationID,
                            Title_Station_AR = result.ApplicationStation.Title_AR,
                            Title_Station_EN = result.ApplicationStation.Title_EN,
                            Direction = result.Direction,
                            Order = result.Order,
                            HelpStation = result.HelpStation,
                        },
                    });
                }
                return Ok(new
                {
                    status = true,
                    description = new
                    {
                        id = 0,
                        RouteId = id,
                        Name_Route_AR = "",
                        Name_Route_EN = "",
                        Area_AR = "",
                        Area_EN = "",
                        StationId = "",
                        Title_Station_AR = "",
                        Title_Station_EN = "",
                        Direction = Direction.Go,
                        Order = 0,
                        HelpStation = "",
                    },
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

        [HttpPost]
        [Route("GetStationOrderInRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> GetStationOrderInRoute(ApplicationRequestId id)
        {

            try
            {
                var result = await _routeStation.GetStationOrderInRoute(id.id);

                if (result != 0)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = 0
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

        [HttpPost]
        [Route("DeleteRouteStationByRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteRouteStationByRoute(ApplicationRequestId id)
        {

            try
            {
                var result = await _routeStation.DeleteRouteStationByRoute(id.id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = result.Message
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

        [HttpPost]
        [Route("ListRouteStation")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _routeStation.List();
                if (pagination.id!=null&&pagination.id!=Guid.Empty)
                {
                    Console.WriteLine("===================");
                    Console.WriteLine(pagination.id);
                    result = result.Where(x => x.RouteID == pagination.id).ToList();
                }
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_AR, Area = x.ApplicationRoute.Area_AR, Title_Station = x.ApplicationStation.Title_AR, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }) :
                        lan == "en" ? result.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_EN, Area = x.ApplicationRoute.Area_EN, Title_Station = x.ApplicationStation.Title_EN, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }) :
                        result.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_EN, Area = x.ApplicationRoute.Area_EN, Title_Station = x.ApplicationStation.Title_EN, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }),
                        total=toto

                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any RouteStation"
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

        [HttpPost]
        [Route("ListStationByRouteID")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ListByRouteID(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _routeStation.List();
                
                result = result.Where(x => x.RouteID == pagination.id).OrderBy(x=>x.Order).ToList();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result.Select(x => new { id = x.id, Title_Station = x.ApplicationStation.Title_AR, Direction = x.Direction, Order = x.Order, Longitude = x.ApplicationStation.Longitude, Latitude = x.ApplicationStation.Latitude, HelpStation = x.HelpStation, }) :
                        lan == "en" ? result.Select(x => new { id = x.id, Title_Station = x.ApplicationStation.Title_EN, Direction = x.Direction, Order = x.Order, Longitude = x.ApplicationStation.Longitude, Latitude = x.ApplicationStation.Latitude, HelpStation = x.HelpStation, }) :
                        result.Select(x => new { id = x.id, Title_Station = x.ApplicationStation.Title_EN, Direction = x.Direction, Order = x.Order, Longitude = x.ApplicationStation.Longitude, Latitude = x.ApplicationStation.Latitude, HelpStation = x.HelpStation, }),
                        total=toto

                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any Station"
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
        [HttpPost]
        [Route("ListRouteByStationID")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ListByStationID(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _routeStation.List();
                
                var result1 = result.Where(x => x.StationID == pagination.id).ToList();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result1 != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result1.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_AR, Area = x.ApplicationRoute.Area_AR, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }) :
                        lan == "en" ? result1.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_EN, Area = x.ApplicationRoute.Area_EN, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }) :
                        result1.Select(x => new { id = x.id, Name_Route = x.ApplicationRoute.Name_EN, Area = x.ApplicationRoute.Area_EN, Direction = x.Direction, Order = x.Order, HelpStation = x.HelpStation, }),
                        total=toto

                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any Route"
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
        [HttpGet]
        [Route("CountRouteStation")]
        public async Task<IActionResult> Count()
        {
            var count = _routeStation.CountRouteStation();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count = count.Result,
                },
            });

        }
    }
}
