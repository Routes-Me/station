using System;
using RoutesStation.Models;

namespace RoutesStation.Interface.Inspection
{
	public interface IInspectionRep
	{
		Task<StatuseModel> AddInspectionBus(ApplicationInspectionBusMap inspectionBusMap);
		Task<StatuseModel> AddInspectionUser(ApplicationInspectionUserMap inspectionUserMap);
		Task<IEnumerable<ApplicationInspectionBusMap>> InspectionBusList();
		Task<IEnumerable<ApplicationInspectionUserMap>> InspectionUserList();
	}
}

