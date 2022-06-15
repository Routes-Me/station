using RoutesStation.Models;

namespace RoutesStation.ModelsView
{
    public class ApplicationRouteStationView
    {
		public Guid id { get; set; }

		public int Order { get; set; }

		public Direction Direction { get; set; }

		public Guid RouteID { get; set; }

		public Guid StationID { get; set; }

		public bool HelpStation { get; set; }
	}
}
