using System;
using RoutesStation.ModelReturn;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Route
{
	public interface IRouteRequestRep
	{
        Task<ApplicationGeneralListWithCountSum<ApplicationRouteRequest>> List(ApplicationPagination pagination);
        Task<StatuseModel> Add(ApplicationRouteRequest route);
        Task<StatuseModel> Edit(ApplicationRouteRequest route);
        Task<ApplicationRouteRequest> Get(Guid id);
        Task<StatuseModel> Delete(Guid id);
        Task<StatuseModel> DeleteRange(List<Guid> id);
    }
}

