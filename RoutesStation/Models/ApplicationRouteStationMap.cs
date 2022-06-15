using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationRouteStationMap
	{
		public ApplicationRouteStationMap()
		{
		}
		[Key]
		public Guid id { get; set; }

		public int Order { get; set; }

		public Direction Direction { get; set; }

		public bool HelpStation { get; set; }

		[ForeignKey("RouteID")]
		public virtual ApplicationRoute ApplicationRoute { get; set; }
		public Guid RouteID { get; set; }

		[ForeignKey("StationID")]
		public virtual ApplicationStation ApplicationStation { get; set; }
		public Guid StationID { get; set; }

		[Display(Name = "Creat Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Creat_Date { get; set; }

	}
	public enum Direction
	{
		Go, Back
	}
}

