namespace Aggregator.Functions.Clients
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using System.Threading.Tasks;
    using Aggregator.Events;

    public static class OnHandClient
    {
        [FunctionName("OnHandClient")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "inventory",
            collectionName: "onHand",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document>
            input, ILogger log,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            foreach (var item in input)
            {
                try
                {
                    //deserializes payload to object and "transform" it
                    string stringifiedItem = item.ToString();
                    InputOnHandEvent inputItem = InputOnHandEvent.Deserialize(stringifiedItem);

                    if (inputItem == null)
                    {
                        throw new InvalidOperationException("InputOnHandEvent not deserialized from payload string");
                    }

                    log.LogInformation(inputItem.Id);
                    InventoryEvent inventoryEvent = InputOnHandEvent.ResolveToInventoryEvent(inputItem);

                    //re-serializes payload and sends it to orchestrator 
                    await starter.StartNewAsync("StoreOrchestrator", null, inventoryEvent).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    log.LogError(e.Message);
                }
            }

        }

    }
}