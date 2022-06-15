using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RoutesStation.Hubs;
using RoutesStation.Interface.Trip;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.API.Trip
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TripAPIController : ControllerBase
    {
        private readonly ITripRep _tripRep;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IHubContext<DashHub> _hub;

        public TripAPIController(ITripRep tripRep, UserManager<ApplicationUser> manager, [NotNull] IHubContext<DashHub> hub)
        {
            _tripRep = tripRep;
            _manager = manager;
            _hub = hub;
        }
        [HttpPost]
        [Route("CreateTrip")]
        [Produces("application/json")]
        public async Task<IActionResult> Add(ApplicationTripView trip)
        {
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                var tripapp = new ApplicationTrip
                {
                    RouteID = trip.RouteID,
                    StartStationID = trip.StartStationID,
                    EndStationID = trip.EndStationID,
                    UserID = user.Id,
                    StartPointLong=trip.StartPointLong,
                    StartPointLut=trip.StartPointLut,
                    EndPointLong=trip.EndPointLong,
                    EndPointLut=trip.EndPointLut,
                    Creat_Date = DateTime.UtcNow,
                };
                var result = await _tripRep.Add(tripapp);
                if (result != null)
                {
                    var t = await _tripRep.List();
                    var t1 = t.FirstOrDefault(x => x.id == new Guid(result.Message));
                    await _hub.Clients.All.SendAsync("TripCount",t.Count().ToString());
                    return Ok(new
                    {
                   
                        status = true,
                        description = new
                        {
                            id=t1.id,
                            RouteName=t1.ApplicationRoute.Name_EN,
                            StartStation=t1.StartStation.Title_EN,
                            EndStation=t1.EndStation.Title_EN,
                            DateTime=t1.Creat_Date
                        }
                });
                }
                return StatusCode(200, new
                {
                    status = false,
                    description = "Can't Crate Trip"
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
        [Route("ListTripByUser")]
        [Produces("application/json")]
        public async Task<IActionResult> ListTripByUser(ApplicationPagination pagination)
        {
            var lan = Request.Headers["lang"];
            try
            {
                var user = await _manager.FindByNameAsync(User.Identity.Name);
                
                var result = await _tripRep.List();
                
                result = result.Where(x => x.UserID == user.Id).ToList();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null) return Ok(new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id=x.id,
                        RoutID=x.RouteID,
                        Rout=lan=="ar"?x.ApplicationRoute.Name_AR:x.ApplicationRoute.Name_EN,
                        Payment=x.ApplicationRoute.Price,
                        StationStartID=x.StartStationID,
                        StartStation= lan == "ar" ? x.StartStation.Title_AR : x.StartStation.Title_EN,
                        StartPointLong=x.StartPointLong,
                        StartPointLut = x.StartPointLut,
                        EndPointLong = x.EndPointLong,
                        EndPointLut = x.EndPointLut,
                        EndStationId =x.EndStationID,
                        EndStation= lan == "ar" ? x.EndStation.Title_AR : x.EndStation.Title_EN,
                        Date=x.Creat_Date,
                        UserID=x.UserID,
                        User=x.User.UserName
                    }),
                    total=toto
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Don't have Trip"
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
        [Route("ListTrip")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "superAdmin,Admin")]
        public async Task<IActionResult> ListTrip(ApplicationPagination pagination)
        {
            var lan = Request.Headers["lang"];
            try
            {
                var result = await _tripRep.List();
                var toto = result.Count();
                result = result.Skip(pagination.PageSize * (pagination.PageNumber - 1))
                          .Take(pagination.PageSize)
                          .ToList();
                if (result != null) return Ok(new
                {
                    status = true,
                    description = result.Select(x => new
                    {
                        id = x.id,
                        RoutID = x.RouteID,
                        Rout = lan == "ar" ? x.ApplicationRoute.Name_AR : x.ApplicationRoute.Name_EN,
                        StationStartID = x.StartStationID,
                        StartStation = lan == "ar" ? x.StartStation.Title_AR : x.StartStation.Title_EN,
                        StartPointLong = x.StartPointLong,
                        StartPointLut = x.StartPointLut,
                        EndPointLong = x.EndPointLong,
                        EndPointLut = x.EndPointLut,
                        EndStationId = x.EndStationID,
                        EndStation = lan == "ar" ? x.EndStation.Title_AR : x.EndStation.Title_EN,

                        Date = x.Creat_Date,
                        UserID = x.UserID,
                        User = x.User.UserName
                    }),
                    total=toto
                });

                return StatusCode(200, new
                {
                    status = false,
                    description = "Don't have Trip"
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
        [Route("CountTrip")]
        public async Task<IActionResult> Count()
        {
            var count = _tripRep.CountTrip();
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