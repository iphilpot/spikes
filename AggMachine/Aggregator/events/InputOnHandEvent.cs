namespace Aggregator.Events
{
    using System;
    using Newtonsoft.Json;

    public class InputOnHandEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("divisionId")]
        public string DivisionId { get; set; }
        [JsonProperty("storeId")]
        public string StoreId { get; set; }
        [JsonProperty("upc")]
        public string Upc { get; set; }
        [JsonProperty("inventoryCount")]
        public int InventoryCount { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("lastUpdateTimestamp")]
        public DateTime? LastUpdateTimestamp { get; set; }


        public static InputOnHandEvent Deserialize(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException("inputString");
            }
            return JsonConvert.DeserializeObject<InputOnHandEvent>(inputString);
        }


        public static InventoryEvent ResolveToInventoryEvent(InputOnHandEvent onHand) => new InventoryEvent()
        {
            Id = onHand.Id,
            DivisionId = onHand.DivisionId,
            StoreId = onHand.StoreId,
            Upc = onHand.Upc,
            CountAdjustment = onHand.InventoryCount,
            ProductName = onHand.ProductName,
            Description = onHand.Description,
            LastUpdateTimestamp = onHand.LastUpdateTimestamp,
            Type = InventoryEventType.OnHandUpdate
        };


    }
}