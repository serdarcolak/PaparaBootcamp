using DigitalSalesPlatform.Business.Job;

namespace DigitalSalesPlatform.Business.RabbitMq;

public interface IRabbitMQService
{
    Task PublishToQueue(EmailMessage message, string queueName);
}