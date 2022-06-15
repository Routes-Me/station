using System;
using RoutesStation.Models;

namespace RoutesStation.Interface.Bus
{
	public interface ICompanyRep
	{
		Task<StatuseModel> Add(ApplicationCompany model);
		Task<StatuseModel> Edite(ApplicationCompany model);
		Task<StatuseModel> Delete(Guid id);
		Task<ApplicationCompany> Get(Guid id);
		Task<IEnumerable<ApplicationCompany>> List();
	}
}

