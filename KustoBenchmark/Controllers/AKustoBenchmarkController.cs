using KustoBenchmark.Helper;
using Microsoft.ApplicationInsights;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http;

namespace KustoBenchmark.Controllers
{
    public class AKustoBenchmarkController : ApiController
    {
        private TelemetryClient telemetry = new TelemetryClient();

        // GET api/AKustoBenchmark
        [SwaggerOperation("Name of the controller")]
        public string Get()
        {
            string query =
                $@"A valid Kusto query";

            QueryHelperAdx.adx.ExecuteQuery("TableName", query, "name of the query", telemetry);

            return "OK";
        }
    }
}
