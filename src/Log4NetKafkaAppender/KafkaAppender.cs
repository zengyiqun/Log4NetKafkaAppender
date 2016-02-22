using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace Log4NetKafkaAppender
{
    public class KafkaAppender : AppenderSkeleton
    {
        public List<string> Brokers { get; set; }
        public string Topic { get; set; }
        public bool WaitForAsync { get; set; }

        private Producer _producer;

        public override void ActivateOptions()
        {
            LogLog.Debug(GetType(), Name + " activating.");
            base.ActivateOptions();
            CheckParameters();
            SetupKafkaProducer();
        }

        private void SetupKafkaProducer()
        {
            var kafkaOptions = new KafkaOptions
            {
                KafkaServerUri = GetBrokerList(Brokers)
            };

            var router = new BrokerRouter(kafkaOptions);
            _producer = new Producer(router);
        }

        private static List<Uri> GetBrokerList(IEnumerable<string> brokers)
        {
            return brokers.Select(broker => new Uri(broker)).ToList();
        }

        private void CheckParameters()
        {
            if (Brokers == null || !Brokers.Any()) throw new Exception("No broker list found.");
            if (string.IsNullOrWhiteSpace(Topic)) throw new Exception("No Topic found.");
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            string message = RenderLoggingEvent(loggingEvent);
            LogLog.Debug(GetType(),
                string.Format("[{0}][{1}] Sending message to Topic `{2}` on Brokers `{3}`. The message is: {4}",
                    loggingEvent.TimeStamp, Name, Topic, string.Join(",", Brokers), message));
            SendMessageToKafka(message);
        }

        private void SendMessageToKafka(string message)
        {
            Task sendTask = _producer.SendMessageAsync(Topic, new Message(message));
            if (WaitForAsync)
            {
                sendTask.Wait();
            }
        }
    }
}
