using Microsoft.Extensions.Configuration;
using Nexmo.Api;
using Nexmo.Api.Request;

namespace Restaurant.Backend.Common.Notifications
{
    public class NexmoNotifications : INexmoNotifications
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        private const string BRAND = "Vonage";
        private const string CODE_LENGTH = "6";
        private const string CANCEL_COMMAND = "cancel";

        public NexmoNotifications(IConfiguration config)
        {
            _apiKey = config.GetSection("AppSettings:Nexmo:ApiKey").Value;
            _apiSecret = config.GetSection("AppSettings:Nexmo:ApiSecret").Value;
        }

        public NumberVerify.VerifyResponse SendNotification(string phoneNumber)
        {
            var client = new Client(new Credentials(_apiKey, _apiSecret));

            return client.NumberVerify.Verify(new NumberVerify.VerifyRequest
            {
                number = phoneNumber,
                brand = BRAND,
                code_length = CODE_LENGTH
            });
        }

        public NumberVerify.ControlResponse CancelNotification(string requestId)
        {
            var client = new Client(new Credentials(_apiKey, _apiSecret));

            return client.NumberVerify.Control(new NumberVerify.ControlRequest
            {
                request_id = requestId,
                cmd = CANCEL_COMMAND
            });
        }

        public NumberVerify.CheckResponse VerifyNotification(string requestId, int code)
        {
            var client = new Client(new Credentials(_apiKey, _apiSecret));
            return client.NumberVerify.Check(new NumberVerify.CheckRequest
            {
                request_id = requestId,
                code = code.ToString()
            });
        }

        public NumberVerify.VerifyResponse CheckStatus(string requestId)
        {
            var credentials = new Credentials(_apiKey, _apiSecret);
            var client = new Client(credentials);
            var request = new NumberVerify.VerifyRequest { Brand = BRAND, Number = requestId };

            return client.NumberVerify.Verify(request, credentials);
        }
    }
}
