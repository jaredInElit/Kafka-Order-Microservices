using Confluent.Kafka;
using Newtonsoft.Json;

namespace Kafka_Ordering_API.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly OrderDatabaseService _databaseService;

        public KafkaConsumerService(ConsumerConfig config, OrderDatabaseService databaseService)
        {
            _config = config;
            _databaseService = databaseService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe("email-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    // Here, you should parse the message and update your MongoDB database
                    var emailEvent = JsonConvert.DeserializeObject<EmailEventDto>(consumeResult.Message.Value);
                    await _databaseService.UpdateEmailStatus(emailEvent.OrderId, emailEvent.Status);

                    Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }

            consumer.Close();
        }
    }
}
