namespace Aggregator.Functions.Clients
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Aggregator.Events;

    public static class InventoryOrchestrator_HttpStart
    {
        [FunctionName("InventoryOrchestrator_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string data = await req.ReadAsStringAsync();
            InventoryEvent incomingEvent;
            try
            {
                incomingEvent = JsonConvert.DeserializeObject<InventoryEvent>(data);
                if(incomingEvent.Type != InventoryEventType.OnHandUpdate && incomingEvent.Type != InventoryEventType.ShipmentUpdate)
                {
                    throw new Exception("Invalid event type. Please specify either 'onHandUpdate' or 'shipmentUpdate'.");
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync<InventoryEvent>("Orchestrator", incomingEvent)
                .ConfigureAwait(false);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var payload = starter.CreateHttpManagementPayload(instanceId);
            return new OkObjectResult(payload);
        }
    }
}