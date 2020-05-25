namespace Aggregator.Events
{
    using System;
    using Newtonsoft.Json;

    public static class InventoryEventType
    {
        public const string OnHandUpdate = "onHandUpdate";
        public const string ShipmentUpdate = "shipmentUpdate";
    }

    public class InventoryEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("divisionId")]
        public string DivisionId { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }

        [JsonProperty("countAdjustment")]
        public int CountAdjustment { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastShipmentTimestamp")]
        public DateTime? LastShipmentTimestamp { get; set; }

        [JsonProperty("lastUpdateTimestamp")]
        public DateTime? LastUpdateTimestamp { get; set; }
    }
}