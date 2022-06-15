using System;
using RoutesStation.Models;

namespace RoutesStation.Interface.PromoterInstallation
{
	public interface IPromoterInstallationRep
	{
        Task<IEnumerable<ApplicationPromoterInstallationMap>> List();
        Task<IEnumerable<ApplicationPromoterInstallationMap>> ListByPromoterID(string PromoterID);
        Task<ApplicationPromoterInstallationMap> GetByUserID(string UserID);
        Task<StatuseModel> Add(ApplicationPromoterInstallationMap station);
    }
}

