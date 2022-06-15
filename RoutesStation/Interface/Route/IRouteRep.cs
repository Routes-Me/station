using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Route
{
    public interface IRouteRep
    {
        Task<IEnumerable<ApplicationRoute>> List();
        Task<StatuseModel> Add(ApplicationRouteView route);
        Task<StatuseModel> Edit(ApplicationRouteView route,Guid id);
        Task<ApplicationRoute> Get(Guid id);
        Task<StatuseModel> Delete(Guid id);
        Task<int> CountRoute();
    }
}
