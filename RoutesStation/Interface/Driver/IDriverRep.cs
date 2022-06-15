using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Driver
{
	public interface IDriverRep
	{
		Task<StatuseModel> Delete(ApplicationUser user);
		Task<StatuseModel> Enter(ApplicationBusDriveView model,ApplicationUser user);
		Task<StatuseModel> Out(ApplicationBusDriveView model, ApplicationUser user);
	}
}

