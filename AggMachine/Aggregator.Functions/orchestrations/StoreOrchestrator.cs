namespace Aggregator.Functions.Orchestrations
{

    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Extensions.Logging;
    using Aggregator.Events;
    using Aggregator.Functions.Entities;

    public static class StoreOrchestrator
    {
        [FunctionName("StoreOrchestrator")]
        public static async Task<Item> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            InventoryEvent data = context.GetInput<InventoryEvent>();

            if (data == null)
            {
                throw new ArgumentException("data received to orchestrator is null");
            }

            var entityId = new EntityId("StoreEntity", data.StoreId);
            var proxy = context.CreateEntityProxy<IStoreEntity>(entityId);
            proxy.Aggregate(data);
            Item sampleItem = await proxy.GetItem(data.Id);

            if (sampleItem != null)
            {
                log.LogInformation(sampleItem.Id);
            }

            return sampleItem;
        }
    }
}