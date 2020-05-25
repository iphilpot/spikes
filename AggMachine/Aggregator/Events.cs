namespace Aggregator.Events
{
    public enum EventType
    {
        Shipment,
        OnHand,
        Resolve
    }

    public interface IEvent
    {
        EventType Type {get;}
        string Data {get; set;}
    }

    public class ShipmentEvent : IEvent 
    {
        public ShipmentEvent()
        {
            Type = EventType.Shipment;
        }
        public EventType Type { get; private set; }
        public string Data { get; set; }
    }

    public class OnHandEvent : IEvent
    {
        public OnHandEvent()
        {
            Type = EventType.OnHand;
        }
        public EventType Type { get; private set; }
        public string Data { get; set; }
    }

    public class ResolveEvent : IEvent
    {
        public ResolveEvent()
        {
            Type = EventType.Resolve;
        }
        public EventType Type { get; private set; }
        public string Data { get; set; }
    }
}