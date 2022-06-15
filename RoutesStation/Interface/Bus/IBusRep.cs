using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Bus
{
	public interface IBusRep
	{
		Task<StatuseModel> Add(ApplicationBus model);
		Task<StatuseModel> Edite(ApplicationBus model);
		Task<StatuseModel> Delete(Guid id);
		Task<ApplicationBus> Get(Guid id);
		Task<IEnumerable<ApplicationBus>> List();
		Task<StatuseModel> ActiveBus(ApplicationSecondID secondID);
		Task<StatuseModel> UnActiveBus(ApplicationSecondID secondID);
	}
}

