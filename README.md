# Log4NetKafkaAppender

A simple log4net appender that you can ask to send logging events to a list of kafka brokers.

It uses the KafkaNetClient https://github.com/gigya/KafkaNetClient

## Sample config

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
  </configSections>
  <log4net>
    <appender name="KafkaAppender" type="Log4NetKafkaAppender.KafkaAppender, Log4NetKafkaAppender">
      <brokers>
        <add value="http://qa.kafka:9092" />
      </brokers>
      <topic>TestTopic</topic>
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="{ &quot;domain&quot;: &quot;%appdomain&quot;, &quot;date&quot;: &quot;%date&quot;, &quot;level&quot;: &quot;%-5level&quot;, &quot;message&quot;: &quot;%message&quot; }" />
      </layout>
    </appender>
    <root>
      <level value="ALL" />
      <appender-ref ref="KafkaAppender" />
    </root>
  </log4net>
</configuration>
```

## Notes

You can define multiple brokers by adding more values to the <brokers> collection.  
You can only send to one topic currently  
The Producer is async, but the appender is not, by default the appender will not wait on the asnyc operation completing. You can set the waitForAsync option in config to true to force a wait on the task.
