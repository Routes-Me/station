using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationEditeUserView
	{
		public ApplicationEditeUserView()
		{
		}
		public string? id { get; set; }

		

		public string? Name { get; set; }


		public string? Email { get; set; }

		public Guid? CompanyID { get; set; }
	}
}

