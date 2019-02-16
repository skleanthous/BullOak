namespace BullOak.Repositories.Appliers
{
    using System;

    internal class ApplierNotFoundException : Exception
    {
        public ApplierNotFoundException(Type typeOfState, Type typeOfEvent)
            : base($"Applier for event {typeOfEvent.Name} for state {typeOfState.Name} was not found or registered. Make sure that any appliers are registered in DI as {typeof(IApplyEvents<>).FullName}")
        { }
        public ApplierNotFoundException(Type typeOfState)
            : base($"No appliers where found for state {typeOfState.Name}. Make sure that any appliers are registered in DI as {typeof(IApplyEvents<>).FullName}")
        { }
    }
}
