using App.Metrics;
using App.Metrics.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workerservice_Consumer.Metrics
{
    public  class MetricsRegistry
    {
        public static CounterOptions _receivedMessage => new CounterOptions
        {
            Name = "Received Messages",
            Context = " Azure Message Service Broker",
            MeasurementUnit = Unit.Calls
        };
    }
}
