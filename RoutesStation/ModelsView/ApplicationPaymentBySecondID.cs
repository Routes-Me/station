using System;
namespace RoutesStation.ModelsView
{
	public class ApplicationPaymentBySecondID
	{
		public ApplicationPaymentBySecondID()
		{
		}
		public string api_key { get; set; }

		public string api_secret { get; set; }

		public double Value { get; set; }

		public Guid? TripID { get; set; }

		public string SecondID { get; set; }

		public string PaymentCode { get; set; }

		public string UserID { get; set; }
	}
}

