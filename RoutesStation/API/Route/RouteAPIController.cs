using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using RoutesStation.Interface.Route;
using RoutesStation.Interface.RouteStation;
using RoutesStation.Interface.Station;
using RoutesStation.ModelsView;

namespace RoutesStation.API
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class RouteAPIController : ControllerBase
    {
        private readonly IRouteRep _route;
        private readonly IStationRep _stationRep;
        private readonly IRouteStationRep _routeStationRep;

        public RouteAPIController(IRouteRep route,IStationRep stationRep,IRouteStationRep routeStationRep)
        {
            _route = route;
            _stationRep = stationRep;
            _routeStationRep = routeStationRep;
        }
        [HttpPost]
        [Route("AddRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationRouteView route)
        {
            try
            {

                    var result = await _route.Add(route);
                    if (result != null) return Ok(new
                    {
                        status = true,
                        description = result
                    });

                    return StatusCode(200, new
                    {
                        status = false,
                        description = "Dont Have Any Route"
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
        [Route("EditRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationRouteView route)
        {
            try
            {

                var result = await _route.Edit(route, route.id);
                if (result != null) return Ok(new
                {
                    status = true,
                    description = result
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Dont Have Any Route"
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
        [Route("DeleteRoute")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete( ApplicationRequestId id)
        {
            try
            {

                var result = await _route.Delete(id.id);

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
        [Route("GetRoute")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {
            try
            {
                var result = await _route.Get(id.id);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id = result.id,
                            Name_AR = result.Name_AR,
                            Name_EN = result.Name_EN,
                            Area_AR = result.Area_AR,
                            Area_EN = result.Area_EN,
                            From_To_EN = result.From_To_EN,
                            From_To_AR = result.From_To_AR,
                            Price = result.Price,
                            Company=result.company,
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
        [Route("ListRoute")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _route.List();
                var toto = result.Count();
                result =result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result.Select(x => new { id = x.id, Name = x.Name_AR, Area = x.Area_AR, From_To = x.From_To_AR, Company = x.company, Price = x.Price, }) :
                        lan == "en" ? result.Select(x => new { id = x.id, Name = x.Name_EN, Area = x.Area_EN, From_To = x.From_To_EN, Company = x.company, Price = x.Price, }) :
                        result.Select(x => new { id = x.id, Name = x.Name_EN, Area = x.Area_EN, From_To = x.From_To_EN, Company = x.company, Price = x.Price, }),
                        total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any route"
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
        [Route("CountRoute")]
        public async Task<IActionResult> CountRoute()
        {
            var count = _route.CountRoute();
            return Ok(new
            {
                status = true,
                description = new
                {
                    Count = count.Result,
                },
            });

        }
        private async Task<int> CountStationInRoute(Guid RouteID)
        {
            var count =await _routeStationRep.List();
            count = count.Where(x => x.RouteID == RouteID);
            Console.WriteLine("==========================================");
            Console.WriteLine(count.Count());
            return count.Count();
            

        }
    }
}

