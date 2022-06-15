using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Account
{
	public interface IRegisterRep
	{
		Task<bool> Edite(ApplicationUser model);
		Task<bool> EditeImage(ApplicationImageView imageView,ApplicationUser user);
		Task<bool> Deactivate(string UserID);
		Task<bool> Activate(string UserID);
		Task<bool> Remove(string UserID);
		Task<bool> ActiveCode(ApplicationUser user,string Code);
		Task<bool> RestUser(ApplicationRestUserView model);

	}
}

