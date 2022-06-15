using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.Models
{
	public class ApplicationRoute
	{
		public ApplicationRoute()
		{
		}
		[Key]
		public Guid id { get; set; }

		[Required]
		public string Name_EN { get; set; }

		public string? Name_AR { get; set; }
		
		public string? From_To_EN { get; set; }//New

		public string? From_To_AR { get; set; }

		public string? Area_EN { get; set; } 

		public string? Area_AR { get; set; }

		public string? company { get; set; }

		public double Price { get; set; }

		


	}
}

