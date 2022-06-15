using System;
using RoutesStation.Models;

namespace RoutesStation.ModelsView
{
	public class ApplicationStationWithDistination
	{
		public ApplicationStationWithDistination()
		{
		}
		public ApplicationRouteStationMap ApplicationRouteStationMap { get; set; }

		public double Distination { get; set; }
	}
}

