using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Station;
using RoutesStation.ModelsView;

namespace RoutesStation.API
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
    public class StationAPIController : ControllerBase
    {
        private readonly IStationRep _station;

        public StationAPIController(IStationRep station)
        {
            _station = station;
        }

        [HttpPost]
        [Route("AddStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationStationView station)
        {

            try
            {
                var result = await _station.Add(station);

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
        [Route("EditStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Edite(ApplicationStationView station)
        {

           try
            {
                if (station.id == null || station.id == Guid.Empty)
                {
                    return Ok(new
                    {
                        status = false,
                        description = "Check Station ID"
                    });
                }
                var result = await _station.Edit(station);

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
        [Route("DeleteStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(ApplicationRequestId id)
        {

            try
            {
                var result = await _station.Delete(id.id);

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
        [Route("GetStation")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(ApplicationRequestId id)
        {

           try
            {
                var result = await _station.Get(id.id);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = new
                        {
                            id=result.id,
                            Title_AR = result.Title_AR,
                            Title_EN = result.Title_EN,
                            Longitude = result.Longitude,
                            Latitude = result.Latitude,
                            Direction=result.DirectionStation

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
        [Route("GetStationByName")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByName(ApplicationSearch key)
        {

            try
            {
                if (key == null || key.keyword == "" || key.keyword.ToString().Count() < 4)
                {
                    return StatusCode(200, new
                    {
                        status = false,
                        description = "You must send less 4 Char"
                    });
                }
                var result = await _station.GetByName(key.keyword);

                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = result.Select(x=> new
                        {
                            id = x.id,
                            Title_AR = x.Title_AR,
                            Title_EN = x.Title_EN,
                            Longitude = x.Longitude,
                            Latitude = x.Latitude,
                            Direction = x.DirectionStation

                        }),
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
        [Route("ListStation")]
        [Produces("application/json")]
        public async Task<IActionResult> List(ApplicationPagination pagination)
        {
            try
            {
                var lan = Request.Headers["lang"];
                var result = await _station.List();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null)
                {
                    return Ok(new
                    {
                        status = true,
                        description = lan == "ar" ? result.Select(x => new { id = x.id, Title = x.Title_AR, Logitude = x.Longitude, Latitude = x.Latitude,Direction=x.DirectionStation }) :
                        lan == "en" ? result.Select(x => new { id = x.id, Title = x.Title_EN, Logitude = x.Longitude, Latitude = x.Latitude, Direction = x.DirectionStation }) :
                        result.Select(x => new { id = x.id, Title = x.Title_EN, Logitude = x.Longitude, Latitude = x.Latitude, Direction = x.DirectionStation }),
                        total=toto
                    });
                }
                return Ok(new
                {
                    status = false,
                    description = "Dont have any station"
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
        [Route("CountStation")]
        public async Task<IActionResult> Count()
        {
            var count = _station.CountStation();
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
