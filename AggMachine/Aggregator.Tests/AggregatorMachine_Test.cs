using System;
using Xunit;
using Aggregator.Machine;

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
            machine.MoveNext(Command.OnHand);
            Assert.Equal(AggregatorState.Aggregating, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Shipment_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            machine.MoveNext(Command.Shipment);
            Assert.Equal(AggregatorState.Aggregating, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Shipment_OnHand_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            machine.MoveNext(Command.Shipment);
            machine.MoveNext(Command.OnHand);
            Assert.Equal(AggregatorState.Resolved, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_OnHand_Shipment_Moves_Aggregating()
        {
            var machine = new AggregatorMachine();
            machine.MoveNext(Command.OnHand);
            machine.MoveNext(Command.Shipment);
            Assert.Equal(AggregatorState.Resolved, machine.CurrentState);
        }

        [Fact]
        public void AggregatorMachine_Resolve_Moves_From_Resolved_Error()
        {
            var machine = new AggregatorMachine();
            machine.MoveNext(Command.OnHand);
            machine.MoveNext(Command.Shipment);
            Assert.Throws<Exception>(() => machine.MoveNext(Command.Resolve));
        }

        [Fact]
        public void AggregatorMachine_Resolve_from_Inactive_Error()
        {
            var machine = new AggregatorMachine();
            Assert.Throws<Exception>(() => machine.MoveNext(Command.Resolve));
        }
    }
}