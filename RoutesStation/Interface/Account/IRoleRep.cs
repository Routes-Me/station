using System;
using RoutesStation.Models;

namespace RoutesStation.Interface.Account
{
	public interface IRoleRep
	{
		Task<StatuseModel> CreateRole();
		Task<StatuseModel> AddRole(string Name);
		Task<StatuseModel> EditeRole(string roleid, string Name);
		Task<StatuseModel> RemoveRole(string roleid);
		Task<ApplicationRole> GetRole(string roleid);
	}
}

