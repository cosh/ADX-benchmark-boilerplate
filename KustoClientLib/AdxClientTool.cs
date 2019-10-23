using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdxClientLib
{
    public class AdxClientTool
    {
        public ICslQueryProvider adx;

        public KustoConnectionStringBuilder connection;
        private static String QUERYTIME = "querytime";
        private static String EXECUTIONS = "executions";
        private static String RESULTCOUNT = "resultCount";

        public AdxClientTool()
        {
            connection =
                new KustoConnectionStringBuilder(Properties.Connection.Default.kustoURL).WithAadApplicationKeyAuthentication(
                applicationClientId: Properties.Connection.Default.appClientId,
                applicationKey: Properties.Connection.Default.appClientSecret,
                authority: Properties.Connection.Default.appAadTenantId);

            adx = KustoClientFactory.CreateCslQueryProvider(connection);
        }

        public void ExecuteQuery(String dbName, string query, String context, TelemetryClient telemetry)
        {
            var queryParameters = new Dictionary<String, String>()
            {
                //{ "xIntValue", "111" },
                // { "xStrValue", "abc" },
                // { "xDoubleValue", "11.1" }
            };

            var clientRequestProperties = new Kusto.Data.Common.ClientRequestProperties(
                options: null,
                parameters: queryParameters);

            clientRequestProperties.ClientRequestId = "Benchmarkapp-" + Guid.NewGuid().ToString();

            int results = 0;

            TelemetryHelper.TrackEvent(EXECUTIONS, context, telemetry);

            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                var queryresult = adx.ExecuteQuery(dbName, query, clientRequestProperties);

                while (queryresult.Read())
                {
                    results++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Failed invoking query '{0}' against Kusto.",
                    query);
                telemetry.TrackException(new ExceptionTelemetry(ex));
                throw ex;
            }

            stopwatch.Stop();

            TelemetryHelper.TrackMetric(RESULTCOUNT, context, telemetry, results);
            TelemetryHelper.TrackMetric(QUERYTIME, context, telemetry, Convert.ToDouble(stopwatch.ElapsedMilliseconds));
        }
    }
}
