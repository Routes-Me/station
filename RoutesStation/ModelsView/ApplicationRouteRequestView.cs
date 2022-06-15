using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationRouteRequestView
	{
		public ApplicationRouteRequestView()
		{
		}
		public Guid id { get; set; }
		[Required]
		public string Name_EN { get; set; }

		public string? Name_AR { get; set; }

		public string? Area_EN { get; set; }

		public string? Area_AR { get; set; }

		public double Price { get; set; }
		
	}
}

