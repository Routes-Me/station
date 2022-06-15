using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RoutesStation.Models
{
	[Index(nameof(Company), IsUnique = true)]
	public class ApplicationCompany
	{
		public ApplicationCompany()
		{
		}
		[Key]
		public Guid id { get; set; }

		public string Company { get; set; }

	}
}

