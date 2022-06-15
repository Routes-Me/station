using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationBusDriverMap
	{
		public ApplicationBusDriverMap()
		{
		}
		[Key]
		public Guid id { get; set; }

		[ForeignKey("BusID")]
		public virtual ApplicationBus ApplicationBus { get; set; }
		public Guid BusID { get; set; }

		[ForeignKey("DriverID")]
		public virtual ApplicationUser ApplicationDriver { get; set; }
		public string DriverID { get; set; }

		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Start_Date { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime End_Date { get; set; }
	}
}

