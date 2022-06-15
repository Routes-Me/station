using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.RouteStation
{
	public interface IRouteStationRep
	{
        Task<IEnumerable<ApplicationRouteStationMap>> List();
        Task<StatuseModel> Add(ApplicationRouteStationView routeStationMap);
        Task<StatuseModel> Edit(ApplicationRouteStationView routeStationMap, Guid id);
        Task<ApplicationRouteStationMap> Get(Guid id);
        Task<int> GetStationOrderInRoute(Guid RouteID);
        
        Task<bool> ReOrderInRoute(Guid RouteID);
        Task<ApplicationRouteStationMap> GetLastRouteStation();
        Task<ApplicationRouteStationMap> GetRouteStationByRouteID(Guid id);
        Task<StatuseModel> Delete(Guid id);
        Task<StatuseModel> DeleteRouteStationByRoute(Guid RouteID);
        Task<int> CountRouteStation();
    }
}

