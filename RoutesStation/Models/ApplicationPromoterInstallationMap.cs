using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoutesStation.Models
{
	//[Index(nameof(UserID), IsUnique = true)]
	public class ApplicationPromoterInstallationMap
	{
		public ApplicationPromoterInstallationMap()
		{
		}

		[Key]
		public Guid id { get; set; }

		[ForeignKey("PromoterID")]
		public virtual ApplicationUser? ApplicationPromoter { get; set; }
		public string? PromoterID { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser? ApplicationUser { get; set; }
		public string? UserID { get; set; }

		[Display(Name = "Installation Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Installation_Date { get; set; }
	}
}

