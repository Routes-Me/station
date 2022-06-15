using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationBusDriveView
	{
		public ApplicationBusDriveView()
		{
		}
		
		public Guid id { get; set; }

		public Guid BusID { get; set; }

		public string? FCMToken { get; set; }
	}
}

