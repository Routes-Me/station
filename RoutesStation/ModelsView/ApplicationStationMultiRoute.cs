using System;
using RoutesStation.Models;

namespace RoutesStation.ModelsView
{
	public class ApplicationStationMultiRoute
	{
		public ApplicationStationMultiRoute()
		{
		}
		public IEnumerable<ApplicationRouteStationMap> Rout1 { get; set; }

		public IEnumerable<ApplicationRouteStationMap> Rout2 { get; set; }

		public ApplicationRouteStationMap StartPoint { get; set; }

		public ApplicationRouteStationMap SharedPoint1 { get; set; }
		public ApplicationRouteStationMap SharedPoint2 { get; set; }

		public ApplicationRouteStationMap EndPoint { get; set; }

		

		public Guid RoutID1 { get; set; }

		public Guid RoutID2 { get; set; }
	}
}

