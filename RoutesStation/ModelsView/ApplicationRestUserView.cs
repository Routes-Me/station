using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationRestUserView
	{
		public ApplicationRestUserView()
		{
		}
		[Required]
		public string UserName { get; set; }

	}
}

