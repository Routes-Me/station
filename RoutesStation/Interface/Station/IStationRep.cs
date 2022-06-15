using System;
using RoutesStation.Models;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Station
{
	public interface IStationRep
	{
        Task<IEnumerable<ApplicationStation>> List();
        Task<StatuseModel> Add(ApplicationStationView station);
        Task<StatuseModel> Edit(ApplicationStationView station);
        Task<ApplicationStation> Get(Guid id);
        Task<IEnumerable<ApplicationStation>> GetByName(string Name);
        Task<StatuseModel> Delete(Guid id);
        Task<int> CountStation();
    }
}

