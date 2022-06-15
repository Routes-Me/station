using System;
using System.ComponentModel.DataAnnotations;

namespace RoutesStation.ModelsView
{
	public class ApplicationConfirmPhoneNumberView
	{
		public ApplicationConfirmPhoneNumberView()
		{
		}
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Code { get; set; }
	}
}

