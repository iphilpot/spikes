namespace Aggregator.Functions.Orchestrations
{

    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Aggregator.Events;

    public static class Orchestrator
    {
        [FunctionName("Orchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {

            // This will ALWAYS read the persisted state from the entity key!
            // Destroy contents of entity's Azure Stor when modifying entity
            try
            {
                InventoryEvent data = context.GetInput<InventoryEvent>();

                if (data == null)
                {
                    throw new ArgumentException("data received to orchestrator is null");
                }

                var entityId = new EntityId("StoreEntity", data.StoreId);
                context.SignalEntity(entityId, "Aggregate", data);

                // Two-way call to the entity which returns a value - awaits the response
                Item sampleItem = await context.CallEntityAsync<Item>(entityId, "GetItem", data.Id).ConfigureAwait(false);

                if (sampleItem != null)
                {
                    log.LogInformation(sampleItem.Id);
                }

            }
            catch (Exception e)
            {
                log.LogError(e.Message);

            }

        }
    }
}