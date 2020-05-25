using System;
using System.Collections.Generic;
using Aggregator.Operations;
using Aggregator.Events;

namespace Aggregator.Machine
{
    public enum AggregatorState
    {
        Inactive,
        Aggregating,
        Resolved
    }

    

    public class AggregatorMachine
    {
        class Transition
        {
            readonly AggregatorState _currentState;
            readonly EventType _eventType;

            public Transition(AggregatorState currentState, EventType eventType)
            {
                _currentState = currentState;
                _eventType = eventType;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(_currentState, _eventType).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Transition other = obj as Transition;
                return other != null && this._currentState == other._currentState && this._eventType == other._eventType;
            }
        }

        Dictionary<Transition, AggregatorState> transitions;
        public AggregatorState CurrentState { get; private set; }
        private Aggregate _aggregate;

        public AggregatorMachine()
        {
            CurrentState = AggregatorState.Inactive;
            _aggregate = new Aggregate();
            transitions = new Dictionary<Transition, AggregatorState>
            {
                { new Transition(AggregatorState.Inactive, EventType.OnHand), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Inactive, EventType.Shipment), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, EventType.OnHand), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, EventType.Shipment), AggregatorState.Aggregating },
                { new Transition(AggregatorState.Aggregating, EventType.Resolve), AggregatorState.Resolved },
            };
        }

        private AggregatorState Next(EventType eventType)
        {
            Transition transition = new Transition(this.CurrentState, eventType);
            AggregatorState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception($"Invalid transition: {this.CurrentState} -> {eventType}.");
            return nextState;
        }

        public void MoveNext(IEvent e)
        {
            this.CurrentState = Next(e.Type);
            IEvent isComplete = _aggregate.Execute(this.CurrentState, e);
            if (isComplete != null) MoveNext(isComplete);
        }
    }
}
