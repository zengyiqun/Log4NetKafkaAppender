using log4net;
using NUnit.Framework;

namespace Log4NetKafkaAppenderTests
{
    class KafkaAppenderTests
    {
        [Test, Explicit("You'll need to set the broker list in App.config to an actual kafka broker to run this test")]
        public void TestLog()
        {
            var logger = LogManager.GetLogger(typeof (KafkaAppenderTests));
            logger.Info("test");
        }
    }
}
