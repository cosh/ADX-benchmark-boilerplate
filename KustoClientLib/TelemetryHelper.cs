using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace AdxClientLib
{
    public class TelemetryHelper
    {
        private static readonly string CONTEXTKEY = "context";

        public static void TrackEvent(string eventName, string context, TelemetryClient telemetry)
        {
            var metric = new MetricTelemetry(eventName, 1);
            metric.Properties.Add(CONTEXTKEY, context);
            telemetry.TrackMetric(metric);
        }

        public static void TrackMetric(string metricName, string context, TelemetryClient telemetry, double value)
        {
            var metric = new MetricTelemetry(metricName, value);
            metric.Properties.Add(CONTEXTKEY, context);
            telemetry.TrackMetric(metric);
        }
    }
}
