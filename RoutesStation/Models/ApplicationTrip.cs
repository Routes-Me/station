using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationTrip
	{
		public ApplicationTrip()
		{
		}
		[Key]
		public Guid id { get; set; }
		
		[ForeignKey("RouteID")]
		public virtual ApplicationRoute? ApplicationRoute { get; set; }
		public Guid? RouteID { get; set; }

		[ForeignKey("StartStationID")]
		public virtual ApplicationStation? StartStation { get; set; }
		public Guid? StartStationID { get; set; }

		public double StartPointLong { get; set; }

		public double StartPointLut { get; set; }

		[ForeignKey("EndStationID")]
		public virtual ApplicationStation? EndStation { get; set; }
		public Guid? EndStationID { get; set; }

		public double EndPointLong { get; set; }

		public double EndPointLut { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser User { get; set; }
		public string UserID { get; set; }

		[Display(Name = "Creat Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Creat_Date { get; set; }

		

	}
}

