using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoutesStation.Interface.Account;
using RoutesStation.Interface.Route;
using RoutesStation.Interface.Station;
using RoutesStation.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RoutesStation.Interface.RouteStation;
using RoutesStation.Interface.Calculate;
using RoutesStation.Interface.Trip;
using RoutesStation.Interface.Wallet;
using RoutesStation.Interface.Upload;
using RoutesStation.Interface.Twillio;
using RoutesStation.Hubs;
using RoutesStation.Interface.Driver;
using RoutesStation.Interface.Bus;
using RoutesStation.Interface.PromoterInstallation;
using RoutesStation.Interface.Inspection;

var builder = WebApplication.CreateBuilder(args);
string MyAllowSpecificOrigins = "*";
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDb>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    //options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<ApplicationDb>().AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.SetIsOriginAllowed(isOriginAllowed: _ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {

                options.SaveToken = true;
                options.Authority = "";
                options.RequireHttpsMetadata = true;
                options.Audience = "myapi";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chathub"))) // for me my hub endpoint is ConnectionHub
                            {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

            });

builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<IRouteRep, RouteRep>();
builder.Services.AddTransient<IStationRep, StationRep>();
builder.Services.AddTransient<IRouteStationRep, RouteStationRep>();
builder.Services.AddTransient<IRoleRep, RoleRep>();
builder.Services.AddTransient<ILoginRep, LoginRep>();
builder.Services.AddTransient<ITowPointRep, TowPointRep>();
builder.Services.AddTransient<IFindPointRep, FindPointRep>();
builder.Services.AddTransient<IRegisterRep, RegisterRep>();
builder.Services.AddTransient<ITripRep, TripRep>();
builder.Services.AddTransient<IWalletRep, WalletRep>();
builder.Services.AddTransient<IUploudRep, UploudRep>();
builder.Services.AddTransient<ITwillioRep, TwillioRep>();
builder.Services.AddTransient<IDriverRep, DriverRep>();
builder.Services.AddTransient<IBusRep, BusRep>();
builder.Services.AddTransient<ICompanyRep, CompanyRep>();
builder.Services.AddTransient<IPromoterInstallationRep, PromoterInstallationRep>();
builder.Services.AddTransient<IInspectionRep, InspectionRep>();
builder.Services.AddTransient<IRouteRequestRep, RouteRequestRep>();







var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseEndpoints(routes =>
{
    routes.MapHub<DashHub>("/chathub");
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

