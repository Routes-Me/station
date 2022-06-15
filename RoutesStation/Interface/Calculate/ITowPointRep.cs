using System;
using RoutesStation.ModelsView;

namespace RoutesStation.Interface.Calculate
{
	public interface ITowPointRep
	{
		Task<double> Dist(ApplicationTwoPointView pointView);
	}
}

