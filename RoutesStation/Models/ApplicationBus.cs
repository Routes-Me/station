using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoutesStation.Models
{
	[Index(nameof(PalteNumber), IsUnique = true)]
	public class ApplicationBus
	{
		public ApplicationBus()
		{
		}
		[Key]
		public Guid id { get; set; }

		[ForeignKey("CompanyID")]
		public virtual ApplicationCompany? ApplicationCompany { get; set; }
		public Guid? CompanyID { get; set; }

		[ForeignKey("RouteID")]
		public virtual ApplicationRoute? ApplicationRoute { get; set; }
		public Guid? RouteID { get; set; }

		public string PalteNumber { get; set; }

		public string? SocondID { get; set; }

		public string? Kind { get; set; }

		[ForeignKey("DriverID")]
		public virtual ApplicationUser? ApplicationDriver { get; set; }
		public string? DriverID { get; set; }

		public bool Active { get; set; }
	}
}

