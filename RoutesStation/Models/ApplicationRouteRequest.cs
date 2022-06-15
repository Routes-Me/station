using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationRouteRequest
	{
		public ApplicationRouteRequest()
		{
		}
		[Key]
		public Guid id { get; set; }
		[Required]
		public string Name_EN { get; set; }

		public string? Name_AR { get; set; }

		public string? Area_EN { get; set; }

		public string? Area_AR { get; set; }

		public bool Read { get; set; }

		[ForeignKey("CompanyID")]
		public virtual ApplicationCompany? ApplicationCompany { get; set; }
		public Guid? CompanyID { get; set; }

		public double Price { get; set; }

		[ForeignKey("AdminID")]
		public virtual ApplicationUser ApplicationAdmin { get; set; }
		public string? AdminID { get; set; }

		[Display(Name = "Request Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Request_Date { get; set; }
	}
}

