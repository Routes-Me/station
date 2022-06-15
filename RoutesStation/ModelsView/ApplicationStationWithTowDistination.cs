using System;
using RoutesStation.Models;

namespace RoutesStation.ModelsView
{
	public class ApplicationStationWithTowDistination
	{
		public ApplicationStationWithTowDistination()
		{
		}
		public ApplicationRouteStationMap ApplicationRouteStationMap { get; set; }

		public double DistinationStart { get; set; }

		public double DistinationEnd { get; set; }
	}
}

