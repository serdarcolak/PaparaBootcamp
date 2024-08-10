using System.Text;
using DigitalSalesPlatform.Business.Job;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DigitalSalesPlatform.Business.RabbitMq
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService(IOptions<RabbitMQSettings> rabbitMqSettings)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Value.HostName,
                Port = rabbitMqSettings.Value.Port,
                UserName = rabbitMqSettings.Value.UserName,
                Password = rabbitMqSettings.Value.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Kuyruğu oluşturma
            _channel.QueueDeclare(queue: "emailQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task PublishToQueue(EmailMessage message, string queueName)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            // Mesajı kuyruğa gönderme
            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}