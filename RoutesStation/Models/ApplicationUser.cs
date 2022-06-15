using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RoutesStation.Models
{
	public class ApplicationUser: IdentityUser
	{
		public ApplicationUser()
		{
		}
		public string? Name { get; set; }
		public string? Image { get; set; }
		public string? Code { get; set; }
		public string? FCMToken { get; set; }
		public string? PaymentCode { get; set; }
		[ForeignKey("CompanyID")]
		public virtual ApplicationCompany? ApplicationCompany { get; set; }
		public Guid? CompanyID { get; set; }

	}
}

