using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationUserView
	{
		public ApplicationUserView()
		{
		}
		
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

		public Guid? CompanyID { get; set; }
	}
}

