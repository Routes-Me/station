using RoutesStation.Models;

namespace RoutesStation.ModelsView
{
    public class ApplicationStationView
    {
        public  Guid id { get; set; }

        public string Title_EN { get; set; }

		public string Title_AR { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }

		public DirectionStation DirectionStation { get; set; }

	}
	
}
