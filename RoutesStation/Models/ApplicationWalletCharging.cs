using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationWalletCharging
	{
		public ApplicationWalletCharging()
		{
		}
		[Key]
		public Guid id { get; set; }

		public double Value { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser User { get; set; }
		public string UserID { get; set; }


		[ForeignKey("PromoterID")]
		public virtual ApplicationUser? ApplicationPromoter { get; set; }
		public string? PromoterID { get; set; }

		[ForeignKey("InspectorID")]
		public virtual ApplicationUser? ApplicationInspector { get; set; }
		public string? InspectorID { get; set; }


		public int? invoiceId { get; set; }

		public string? paymentGateway { get; set; }

		public TypeCharge TypeCharge { get; set; }


		[Display(Name = "Payment Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Payment_Date { get; set; }
	}
	public enum TypeCharge
    {
		cart,Promoter,Inspector
	}
}

