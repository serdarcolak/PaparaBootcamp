namespace DigitalSalesPlatform.Business.Notification;

public interface INotificationService
{
    Task SendEmail(string to, string subject, string body);
}