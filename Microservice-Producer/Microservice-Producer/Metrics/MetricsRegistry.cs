using App.Metrics;
using App.Metrics.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.Metrics
{
    public class MetricsRegistry
    {
        public static CounterOptions _sentMessage => new CounterOptions
        {
            Name = "Sent Messages",
            Context = " Azure Message Service Broker",
            MeasurementUnit = Unit.Calls
        };
    }
}
