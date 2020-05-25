using System;
using System.Collections.Generic;

namespace Aggregator.Machine
{
    public enum AggregatorState
    {
        Inactive,
        Aggregating,
        Resolved
    }

    public enum Command
    {
        Shipment,
        OnHand,
        Resolve
    }

    public class AggregatorMachine
    {
        class Transition
        {
            readonly AggregatorState _currentState;
            readonly Command _command;

            public Transition(AggregatorState currentState, Command command)
            {
                _currentState = currentState;
                _command = command;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(_currentState, _command).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Transition other = obj as Transition;
                return other != null && this._currentState == other._currentState && this._command == other._command;
            }
        }

        Dictionary<Transition, AggregatorState> transitions;
        public AggregatorState CurrentState { get; private set; }

        private bool shipmentReceived;
        private bool onHandReceived;

        public AggregatorMachine()
        {
            CurrentState = AggregatorState.Inactive;
            transitions = new Dictionary<Transition, AggregatorState>
            {
                { new Transition(AggregatorState.Inactive, Command.OnHand), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Inactive, Command.Shipment), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, Command.OnHand), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, Command.Shipment), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, Command.Resolve), AggregatorState.Resolved },
            };
        }

        private AggregatorState Next(Command command)
        {
            Transition transition = new Transition(this.CurrentState, command);
            AggregatorState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception($"Invalid transition: {this.CurrentState} -> {command}.");
            return nextState;
        }

        public void MoveNext(Command command)
        {
            this.CurrentState = Next(command);

            switch (this.CurrentState)
            {
                case AggregatorState.Aggregating:
                    if (command == Command.Shipment) this.shipmentReceived = true;
                    if (command == Command.OnHand) this.onHandReceived = true;
                    if (this.onHandReceived && this.shipmentReceived) MoveNext(Command.Resolve);
                    break;
                default:
                    break;
            }
        }
    }
}
