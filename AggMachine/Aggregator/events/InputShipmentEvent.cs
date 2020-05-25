namespace Aggregator.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class ShipmentItem
    {
        [JsonProperty("upc")]
        public string upc { get; set; }
        [JsonProperty("shipmentAmount")]
        public int shipmentAmount { get; set; }
    }

    public class InputShipmentEvent
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("divisionId")]
        public string divisionId { get; set; }
        [JsonProperty("storeId")]
        public string storeId { get; set; }
        [JsonProperty("distributorId")]
        public string distributorId { get; set; }
        [JsonProperty("items")]
        public List<ShipmentItem> items { get; set; }
        [JsonProperty("arrivalTimeStamp")]
        public DateTime? arrivalTimestamp { get; set; }

        public static InputShipmentEvent Deserialize(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException("inputString");
            }
            return JsonConvert.DeserializeObject<InputShipmentEvent>(inputString);
        }

        public static List<InventoryEvent> ResolveToInventoryEvents(InputShipmentEvent shipment)
        {
            return shipment.items.Select(i => new InventoryEvent
            {
                Id = $"{shipment.divisionId}:{shipment.storeId}:{i.upc}",
                DivisionId = shipment.divisionId,
                StoreId = shipment.storeId,
                LastShipmentTimestamp = shipment.arrivalTimestamp,
                LastUpdateTimestamp = shipment.arrivalTimestamp,
                Upc = i.upc,
                CountAdjustment = i.shipmentAmount,
                Type = InventoryEventType.ShipmentUpdate
            }).ToList();
        }
    }
}