using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoutesStation.Models
{
	
	//[Index(nameof(Title_AR), IsUnique = true)]
	public class ApplicationStation
	{
		public ApplicationStation()
		{
		}
		[Key]
		public Guid id { get; set; }

		[Required]
		public string Title_EN { get; set; }

		public string? Title_AR { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }

		public DirectionStation DirectionStation { get; set; }


	}
	public enum DirectionStation
	{
		R,L
	}

}

