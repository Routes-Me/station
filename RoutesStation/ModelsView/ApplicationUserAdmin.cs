using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationUserAdmin
	{
		public ApplicationUserAdmin()
		{
		}
		[Required]
		public string UserName { get; set; }

		
		public string? email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}

