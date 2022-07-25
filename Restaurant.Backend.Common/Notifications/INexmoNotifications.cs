using Nexmo.Api;

namespace Restaurant.Backend.Common.Notifications
{
    public interface INexmoNotifications
    {
        NumberVerify.VerifyResponse SendNotification(string phoneNumber);
        NumberVerify.ControlResponse CancelNotification(string requestId);
        NumberVerify.CheckResponse VerifyNotification(string requestId, int code);
        NumberVerify.VerifyResponse CheckStatus(string requestId);
    }
}