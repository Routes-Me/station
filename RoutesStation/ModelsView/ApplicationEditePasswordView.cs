using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationEditePasswordView
	{
		public ApplicationEditePasswordView()
		{
		}
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		public string Password { get; set; }
	}
}

