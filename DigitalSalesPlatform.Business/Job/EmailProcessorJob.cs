using System.Text;
using DigitalSalesPlatform.Business.Notification;
using DigitalSalesPlatform.Business.RabbitMq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DigitalSalesPlatform.Business.Job;

public class EmailProcessorJob
{
    private readonly INotificationService _notificationService;
    private readonly RabbitMQSettings _rabbitMQSettings;

    public EmailProcessorJob(INotificationService notificationService,  RabbitMQSettings rabbitMQSettings)
    {
        this._notificationService = notificationService;
        this._rabbitMQSettings = rabbitMQSettings;
    }

    public void ProcessEmailQueue()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.HostName,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(message);

                _notificationService.SendEmail(emailMessage.Email, emailMessage.Subject, emailMessage.Body).Wait();
            };

            channel.BasicConsume(queue: "emailQueue", autoAck: true, consumer: consumer);
        }
    }
}

public class EmailMessage
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}