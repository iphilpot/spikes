namespace Aggregator.Functions.Clients
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using System.Threading.Tasks;

    public static class OnHandUpdateFunc
    {
        [FunctionName("OnHandUpdateFunc")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "inventory",
            collectionName: "onHand",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document>
            input, ILogger log,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            if (input == null)
            {
                log.LogWarning("Client has received no documents");
                return;
            }

            //todo: determine whether or not we want to keep the rest of the payload if a singular document is invalid
            //note: throwing an error here would "pause" the position of where we're processing
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
                    await starter.StartNewAsync("Orchestrator", null, inventoryEvent).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    log.LogError(e.Message);
                }
            }

        }

    }
}