namespace Aggregator.Functions.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Aggregator.Events;

    public static class ShipmentClient
    {
        [FunctionName("ShipmentClient")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "inventory",
            collectionName: "shipments",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            ILogger log, [DurableClient] IDurableOrchestrationClient starter)
        {
            if (input == null)
            {
                log.LogWarning("Client has received no documents");
                return;
            }

            foreach (var item in input)
            {
                try
                {
                    //deserializes payload to object and "transform" it
                    string stringifiedItem = item.ToString();

                    InputShipmentEvent inputItem = InputShipmentEvent.Deserialize(stringifiedItem);
                    if (inputItem == null)
                    {
                        throw new InvalidOperationException("InputOnHandEvent not deserialized from payload string");
                    }

                    log.LogInformation(inputItem.id);

                    //re-serializes payload and sends it to orchestrator 
                    var inventoryEvents = InputShipmentEvent.ResolveToInventoryEvents(inputItem);
                    foreach (var invEvent in inventoryEvents)
                    {
                        string instanceId = await starter.StartNewAsync("StoreOrchestrator", null, invEvent).ConfigureAwait(false);

                        var mgmtUrls = starter.CreateHttpManagementPayload(instanceId);
                        log.LogInformation(inputItem.id, mgmtUrls.ToString());
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e.Message);
                }
            }
        }
    }
}