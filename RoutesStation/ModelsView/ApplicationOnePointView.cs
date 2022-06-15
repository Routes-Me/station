using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationOnePointView
	{
		public ApplicationOnePointView()
		{
		}
		[Required]
		public double Longitude { get; set; }

		[Required]
		public double Latitude { get; set; }
	}
}

