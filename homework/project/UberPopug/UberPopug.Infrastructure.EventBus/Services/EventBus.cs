using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UberPopug.Infrastructure.EventBus.API;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UberPopug.Infrastructure.EventBus.Services;

public class EventBus : IEventBus, IDisposable
{
    private const string exchangeName = "broker_exchange";
    private readonly IModel channel;
    private readonly IConnection connection;
    private readonly ConnectionFactory connectionFactory;
    private readonly EventHandlersCollection eventHandlers;
    private readonly IServiceProvider serviceProvider;

    public EventBus(IServiceProvider serviceProvider, string hostName, int port, string username, string password)
    {
        this.serviceProvider = serviceProvider;
        connectionFactory = new ConnectionFactory
        {
            HostName = hostName, Port = port, UserName = username, Password = password, DispatchConsumersAsync = true
        };
        connection = connectionFactory.CreateConnection();
        channel = connection.CreateModel();
        eventHandlers = new EventHandlersCollection();
    }

    public void Dispose()
    {
        channel.Dispose();
        connection.Dispose();
    }

    public void Publish<T>(T eventModel) where T : IEventModel
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        var message = JsonConvert.SerializeObject(eventModel);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchangeName,
            eventModel.GetType().Name,
            null,
            body);
    }

    public void Subscribe<T, TH>()
        where T : IEventModel
        where TH : IEventHandler<T>
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        var queueName = channel.QueueDeclare("someApp", exclusive: false).QueueName;
        channel.QueueBind(queueName, exchangeName, typeof(T).Name);
        eventHandlers.AddHandler<T, TH>();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += ConsumerReceived;

        channel.BasicConsume(
            queue: queueName,
            autoAck: true,
            consumer: consumer,
            exclusive: false);
    }

    private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        var handlers = eventHandlers.FindHandlersForEvent(eventName);

        foreach (var handlerType in handlers)
        {
            var eventType = eventHandlers.GetEventTypeByKey(eventName);
            var eventObject = JsonSerializer.Deserialize(message, eventType,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var handler = ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

            await Task.Yield();
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { eventObject });
        }
    }
}