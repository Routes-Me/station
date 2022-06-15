using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutesStation.Models
{
	public class ApplicationWallet
	{
		public ApplicationWallet()
		{
		}

		[Key]
		public Guid id { get; set; }


		public double Total { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser User { get; set; }
		public string UserID { get; set; }
	}
}

