using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationInspectionUserMap
	{
		public ApplicationInspectionUserMap()
		{
		}
		[Key]
		public Guid id { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser ApplicationUser { get; set; }
		public string? UserID { get; set; }

		[ForeignKey("InspectorID")]
		public virtual ApplicationUser ApplicationInspector { get; set; }
		public string? InspectorID { get; set; }

		[Display(Name = "Creat Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Creat_Date { get; set; }
	}
}

