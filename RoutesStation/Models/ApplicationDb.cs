using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RoutesStation.Models
{
	public class ApplicationDb : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options) { }

        public DbSet<ApplicationRoute> Routes { get; set; }

        public DbSet<ApplicationRouteRequest> RouteRequests { get; set; }

        

        public DbSet<ApplicationStation> Stations { get; set; }

        public DbSet<ApplicationRouteStationMap> RouteStationMaps { get; set; }

        public DbSet<ApplicationTrip> Trips { get; set; }


        public DbSet<ApplicationWallet> Wallets { get; set; }

        public DbSet<ApplicationWalletCharging> WalletChargings { get; set; }

        public DbSet<ApplicationWalletTrip> WalletTrips { get; set; }

        public DbSet<ApplicationBus> Buses { get; set; }

        public DbSet<ApplicationBusDriverMap> BusDriverMaps { get; set; }

        public DbSet<ApplicationCompany> Companies { get; set; }

        public DbSet<ApplicationPromoterInstallationMap> PromoterInstallationMaps { get; set; }

        public DbSet<ApplicationInspectionBusMap> InspectionBusMaps { get; set; }

        public DbSet<ApplicationInspectionUserMap> InspectionUserMaps { get; set; }

        
            







    }
}

