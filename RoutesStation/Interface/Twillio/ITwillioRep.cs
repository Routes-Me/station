using System;
using RoutesStation.Models;

namespace RoutesStation.Interface.Twillio
{
	public interface ITwillioRep
	{
		Task<StatuseModel> SendSMS(string Mobile, string Code);
	}
}

