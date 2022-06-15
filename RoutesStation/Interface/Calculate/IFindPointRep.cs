using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Calculate
{
	public interface IFindPointRep
	{
		Task<IEnumerable<ApplicationStationWithDistination>> FindPoint(ApplicationOnePointView pointView);
		Task<ApplicationStationMultiRoute> FindSharedPoint(IEnumerable<ApplicationStationWithDistination> point1, IEnumerable<ApplicationStationWithDistination> point2);
		Task<List<ApplicationRouteStationMap>> FindRoute(ApplicationRouteStationMap StartStation, ApplicationRouteStationMap EndStation);
		
	}
}

