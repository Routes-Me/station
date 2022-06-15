using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationPaymentWalletView
	{
		public ApplicationPaymentWalletView()
		{
		}


		public string api_key { get; set; }

		public string api_secret { get; set; }

		public double Value { get; set; }

		public Guid? TripID { get; set; }

		public Guid? BusID { get; set; }

		public string? UserID { get; set; }
	}
}

