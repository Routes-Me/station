using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationTripView
	{
		public ApplicationTripView()
		{
		}

		public Guid id { get; set; }

		
		public Guid RouteID { get; set; }

		
		public Guid StartStationID { get; set; }

		
		public Guid EndStationID { get; set; }

		public double StartPointLong { get; set; }

		public double StartPointLut { get; set; }

		public double EndPointLong { get; set; }

		public double EndPointLut { get; set; }
	}
}

