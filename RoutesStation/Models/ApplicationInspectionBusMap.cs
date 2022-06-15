using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationInspectionBusMap
	{
		public ApplicationInspectionBusMap()
		{
		}
		[Key]
		public Guid id { get; set; }

		[ForeignKey("BusID")]
		public virtual ApplicationBus ApplicationBus { get; set; }
		public Guid? BusID { get; set; }

		[ForeignKey("InspectorID")]
		public virtual ApplicationUser ApplicationInspector { get; set; }
		public string? InspectorID { get; set; }

		[Display(Name = "Creat Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Creat_Date { get; set; }
	}
}

