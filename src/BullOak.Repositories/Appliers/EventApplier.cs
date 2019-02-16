namespace BullOak.Repositories.Appliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices.ComTypes;
    using BullOak.Repositories.StateEmit;

    internal class EventApplier : IApplyEventsToStates
    {
        private IConstructScopedResolver scopedResolverFactory;

        public object Apply(Type stateType, dynamic state, IEnumerable<dynamic> events)
        {
            var switchable = state as ICanSwitchBackAndToReadOnly;
            if (switchable != null) switchable.CanEdit = true;

            using (var scopedResolver = scopedResolverFactory.GetScopedResolver())
            {
                foreach (var @event in events)
                {
                    dynamic eventApplier = scopedResolver.Resolve(GetEventApplierTypeFor(state, @event));

                    if(eventApplier == null)
                        throw new ApplierNotFoundException(stateType, @event.GetType());

                    @state = eventApplier.Apply(state, @event);
                }
            }

            if (switchable != null) switchable.CanEdit = false;

            return state;
        }

        private static Type GetEventApplierTypeFor<TState, TEvent>(TState state, TEvent @event)
            => typeof(IApplyEvent<TState, TEvent>);
    }
}
