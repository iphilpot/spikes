using Aggregator.Machine;
using Aggregator.Events;

namespace Aggregator.Operations
{
    public class Aggregate
    {
        private bool shipmentReceived;
        private bool onHandReceived;
        private string aggregatedData;
        public IEvent Execute(AggregatorState currentState, IEvent e)
        {
            switch (currentState)
            {
                case AggregatorState.Aggregating:
                    if (e.Type == EventType.Shipment) this.shipmentReceived = true;
                    if (e.Type == EventType.OnHand) this.onHandReceived = true;

                    this.aggregatedData += e.Data;
                    if (this.onHandReceived && this.shipmentReceived)
                    {
                        return new ResolveEvent
                        {
                            Data = this.aggregatedData
                        };
                    }

                    break;
                default:
                    break;
            }

            return null;
        }
    }
}