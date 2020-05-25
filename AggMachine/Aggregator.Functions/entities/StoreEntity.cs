namespace Name
{
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using System.Collections.Generic;
    using Aggregator.Functions.Entities;

    [JsonObject(MemberSerialization.OptIn)]
    public class StoreEntity : IStoreEntity
    {
        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("divisionId")]
        public string DivisionId { get; set; }

        [JsonProperty("items")]
        public Dictionary<string, Item> Items = new Dictionary<string, Item>();

        [FunctionName(nameof(StoreEntity))]
        public static Task Store([EntityTrigger] IDurableEntityContext ctx)
             => ctx.DispatchAsync<StoreEntity>();

        public Task<Item> GetItem(string itemId) => this.Items.ContainsKey(itemId) ? Task.FromResult(this.Items[itemId]) : null;

        private void OnHandUpdate(InventoryEvent incomingEvent, Item currentItem)
        {
            currentItem.InventoryCount = incomingEvent.CountAdjustment;
            currentItem.ProductName = incomingEvent.ProductName;
            currentItem.Description = incomingEvent.Description;
            currentItem.LastUpdateTimestamp = incomingEvent.LastUpdateTimestamp;
        }

        private void ShipmentUpdate(InventoryEvent incomingEvent, Item currentItem)
        {
            currentItem.InventoryCount += incomingEvent.CountAdjustment;
            currentItem.LastShipmentTimestamp = incomingEvent.LastShipmentTimestamp;
            currentItem.LastUpdateTimestamp = incomingEvent.LastUpdateTimestamp;
        }

        public void Aggregate(InventoryEvent inventoryEvent)
        {
            if (this.Items.TryGetValue(inventoryEvent.Id, out Item item))
            {
                // Updates dict record if item is found
                // Event types are validated in the orchestrator client so we will always get one of these two types
                if (inventoryEvent.Type == InventoryEventType.OnHandUpdate)
                {
                    OnHandUpdate(inventoryEvent, item);
                }
                else if (inventoryEvent.Type == InventoryEventType.ShipmentUpdate)
                {
                    ShipmentUpdate(inventoryEvent, item);
                }
            }
            else
            {
                //Creates new record in the dict with newly found item
                this.Items[inventoryEvent.Id] = new Item()
                {
                    Id = inventoryEvent.Id,
                    Upc = inventoryEvent.Upc,
                    InventoryCount = inventoryEvent.CountAdjustment,
                    ProductName = inventoryEvent.ProductName,
                    Description = inventoryEvent.Description,
                    LastShipmentTimestamp = inventoryEvent.LastShipmentTimestamp,
                    LastUpdateTimestamp = inventoryEvent.LastUpdateTimestamp
                };
            }
        }
    }
}