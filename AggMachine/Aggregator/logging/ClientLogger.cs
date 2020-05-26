namespace Aggregator.Logging
{
     using Microsoft.Extensions.Logging;
     using Newtonsoft.Json;
     using Microsoft.Azure.WebJobs.Extensions.DurableTask;

    public class ClientLogger
    {
        public ILogger Log { get; private set; }

        public ClientLogger(ILogger log)
        {
            Log = log;
        }

        public void LogManagmentUrls(IDurableOrchestrationClient starter, string instanceId)
        {
            var mgmtUrls = starter.CreateHttpManagementPayload(instanceId);
            var mgmtString = JsonConvert.SerializeObject(mgmtUrls);
            Log.LogInformation(mgmtString);
        }

        public void LogOrchestrationStarted(string instanceId)
        {
            Log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}