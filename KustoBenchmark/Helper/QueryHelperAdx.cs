using AdxClientLib;

namespace KustoBenchmark.Helper
{
    internal class QueryHelperAdx
    {
        public static AdxClientTool adx = CreateClient();

        private static AdxClientTool CreateClient()
        {
            return new AdxClientTool();
        }
    }
}