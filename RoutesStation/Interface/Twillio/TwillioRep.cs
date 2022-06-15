using System;
using Vonage;
using Vonage.Request;
using RoutesStation.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RoutesStation.Interface.Twillio
{
	public class TwillioRep:ITwillioRep
	{
		public TwillioRep()
		{
		}

        public async Task<StatuseModel> SendSMS(string Mobile,string Code)
        {
            
            HttpClient client = new HttpClient();
            var mobile = Mobile;
            var message = "Routes Application"+ Environment.NewLine + "Please enter this code to verify your phone number = "+Code;
            string url= "https://www.kwtsms.com/API/send/?username=routes&password=routes_@!3015&sender=KWT-MESSAGE&mobile="+mobile+ "&lang=2&message="+message;
            var responseMessage = await client.GetAsync(url);
            var contentStream = await responseMessage.Content.ReadAsStringAsync();
            
            
            var faild = new StatuseModel
            {
                Status = true,
                Message = contentStream
            };
            return faild;
        }
    }
}

