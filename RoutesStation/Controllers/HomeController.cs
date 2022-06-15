using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoutesStation.Interface.Account;
using RoutesStation.Models;

namespace RoutesStation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoleRep _roleRep;
    private readonly RoleManager<ApplicationRole> _role;

    public HomeController(ILogger<HomeController> logger, IRoleRep roleRep, RoleManager<ApplicationRole> role)
    {
        _roleRep = roleRep;
        _role = role;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        if (_role.Roles.Count() < 1)
        {
            await _roleRep.CreateRole();
        }
        return View();
    }

    /*public IActionResult Privacy()
    {
        return View();
    }*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

