using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationWalletTrip
	{
		public ApplicationWalletTrip()
		{
		}
		[Key]
		public Guid id { get; set; }


		public double Value { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser ApplicationUser { get; set; }
		public string UserID { get; set; }

		[ForeignKey("InspectorID")]
		public virtual ApplicationUser? ApplicationInspector { get; set; }
		public string? InspectorID { get; set; }

		[ForeignKey("TripID")]
		public virtual ApplicationTrip? ApplicationTrip { get; set; }
		public Guid? TripID { get; set; }

		[ForeignKey("BusID")]
		public virtual ApplicationBus? ApplicationBus { get; set; }
		public Guid? BusID { get; set; }

		[ForeignKey("DriverID")]
		public virtual ApplicationUser? ApplicationDriver { get; set; }
		public string? DriverID { get; set; }

		//String? busId;
		//String? driverId;


		[Display(Name = "Payment Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Payment_Date { get; set; }
	}
}

