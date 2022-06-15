using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationTwoPointView
	{
		public ApplicationTwoPointView()
		{
		}
		[Required]
		public double Longitude1 { get; set; }

		[Required]
		public double Latitude1 { get; set; }

		[Required]
		public double Longitude2 { get; set; }

		[Required]
		public double Latitude2 { get; set; }
	}
}

