using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RoutesStation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RoutesStation.Hubs
{
    public class DashHub:Hub
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _manager;

        public DashHub( ApplicationDb db, UserManager<ApplicationUser> manager)
        {
            _db = db;
            _manager = manager;

        }
        public override Task OnConnectedAsync()
        {
            //Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();

        }
    }
}
