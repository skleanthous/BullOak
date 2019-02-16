namespace BullOak.Repositories.Appliers
{
    using System;
    using System.Collections.Generic;
    using BullOak.Repositories.Upconverting;

    public interface IApplyEventsToStates
    {
        object Apply(Type stateType, object state, IEnumerable<object> events);
        object ApplyEvent(Type stateType, object state, object @event);
    }
}