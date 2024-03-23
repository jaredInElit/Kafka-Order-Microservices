using Confluent.Kafka;

namespace Kafka_Ordering_API.Services
{
    public class KafkaProducerService
    {
        private readonly ProducerConfig _config;

        public KafkaProducerService(ProducerConfig config)
        {
            _config = config;
        }

        public async Task ProduceMessageAsync(string topic, string message)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            try
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                producer.Flush(TimeSpan.FromSeconds(10));
            }
            catch (ProduceException<Null, string> e)
            {
                throw new Exception($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
