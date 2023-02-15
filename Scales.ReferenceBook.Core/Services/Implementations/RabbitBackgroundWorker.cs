using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Scales.ReferenceBook.Core.Options;
using Scales.ReferenceBook.Core.Services.Interfaces;
using SharedLibrary.DTO.ReferenceBook;
using System.Text;

namespace Scales.ReferenceBook.Core.Services.Implementations
{
    public class RabbitBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqOptions _rabbitMqOptions;
        public RabbitBackgroundWorker(IServiceProvider serviceProvider, IOptions<RabbitMqOptions> options)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqOptions = options.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = RabbitMqFactory.CreateChannel(hostName: _rabbitMqOptions.HostName, port: _rabbitMqOptions.Port);
            channel.ExchangeDeclare(exchange: "refBook", ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "refBook", routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var refTransportDto = System.Text.Json.JsonSerializer.Deserialize<RefTransportDto>(message);
                if (refTransportDto != null)
                {
                    using var _scope = _serviceProvider.CreateScope();
                    var refTransportService = _scope.ServiceProvider.GetRequiredService<IRefTransportService>();
                    await refTransportService.CreateRefTransportAsync(refTransportDto, stoppingToken);
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            await Task.CompletedTask;
        }
    }
}
