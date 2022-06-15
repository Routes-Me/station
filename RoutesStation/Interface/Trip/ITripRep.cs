using System;
using RoutesStation.ModelReturn;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Trip
{
	public interface ITripRep
	{
		Task<IEnumerable<ApplicationTrip>> List();
		Task<ApplicationGeneralListWithCountSum<ApplicationTrip>>ListByUser(ApplicationPagination pagination);
		Task<StatuseModel> Add(ApplicationTrip station);
		Task<int> CountTrip();
	}
}

