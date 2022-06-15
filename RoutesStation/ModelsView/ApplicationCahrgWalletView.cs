using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationCahrgWalletView
	{
		public ApplicationCahrgWalletView()
		{
		}
		public string api_key { get; set; }

		public string api_secret { get; set; }

		public string? UserID { get; set; }

		public double invoiceValue { get; set; }

		public int? invoiceId { get; set; }

		public string? paymentGateway { get; set; }
	}
}

