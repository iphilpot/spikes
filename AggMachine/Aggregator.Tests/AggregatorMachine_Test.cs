using System;
using Xunit;
using Aggregator.Machine;
using Aggregator.Events;

namespace Aggregator.UnitTests.Machines
{
    public class AggregatorMachine_Test
    {
        [Fact]
        public void AggregatorMachine_ShouldStartInactive()
        {
            var machine = new AggregatorMachine();
            Assert.Equal(AggregatorState.Inactive, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_OnHand_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            IEvent e = new OnHandEvent { Data = "Hello " };
            machine.MoveNext(e);
            Assert.Equal(AggregatorState.Aggregating, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Shipment_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            IEvent e = new ShipmentEvent { Data = "world!" };
            machine.MoveNext(e);
            Assert.Equal(AggregatorState.Aggregating, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Shipment_OnHand_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            IEvent onHandEvent = new OnHandEvent { Data = "Hello " };
            IEvent shipmentEvent = new ShipmentEvent { Data = "world!" };
            machine.MoveNext(shipmentEvent);
            machine.MoveNext(onHandEvent);
            Assert.Equal(AggregatorState.Resolved, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_OnHand_Shipment_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            IEvent onHandEvent = new OnHandEvent { Data = "Hello " };
            IEvent shipmentEvent = new ShipmentEvent { Data = "world!" };
            machine.MoveNext(onHandEvent);
            machine.MoveNext(shipmentEvent);
            Assert.Equal(AggregatorState.Resolved, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Resolve_Moves_From_Resolved_Error()
        {
            var machine = new AggregatorMachine();
            IEvent onHandEvent = new OnHandEvent { Data = "Hello " };
            IEvent shipmentEvent = new ShipmentEvent { Data = "world!" };
            IEvent resolveEvent = new ResolveEvent { Data = "Hello world?" };
            machine.MoveNext(onHandEvent);
            machine.MoveNext(shipmentEvent);
            Assert.Throws<Exception>(() => machine.MoveNext(resolveEvent));
        }

        [Fact]
        public void AggregatorMachine_Resolve_from_Inactive_Error()
        {
            var machine = new AggregatorMachine();
            IEvent resolveEvent = new ResolveEvent { Data = "Hello world?" };
            Assert.Throws<Exception>(() => machine.MoveNext(resolveEvent));
        }
    }
}