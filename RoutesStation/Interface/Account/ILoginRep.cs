using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Account
{
	public interface ILoginRep
	{
		Task<StatuseModel> LoginUser(ApplicationUserView loginModel);
	}
}

