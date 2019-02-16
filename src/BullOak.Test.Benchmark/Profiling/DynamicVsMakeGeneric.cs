namespace BullOak.Test.Benchmark.Profiling
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Jobs;
    using BullOak.Repositories.Appliers;

    public class AnEvent { }
    public class TheState { }

    [ShortRunJob]
    public class DynamicVsMakeGeneric
    {
        private AnEvent theEvent = new AnEvent();
        private TheState theState = new TheState();

        private Type anEventType = typeof(AnEvent);
        private Type theStateType = typeof(TheState);

        private Type openApplyInterface = typeof(IApplyEvent<,>);

        private Dictionary<Tuple<Type, Type>, Type> ResolvedTypes = new Dictionary<Tuple<Type, Type>, Type>();


        [IterationSetup]
        public void Setup()
        {
            var key = new Tuple<Type, Type>(theStateType, anEventType);

            if (!ResolvedTypes.ContainsKey(key))
                ResolvedTypes.Add(key, MakeGeneric());
        }

        public Type MakeGeneric()
            => openApplyInterface.MakeGenericType(theStateType, anEventType);

        [Benchmark]
        public object FindFromDictionary()
        {
            var key = new Tuple<Type, Type>(theStateType, anEventType);

            if (!ResolvedTypes.TryGetValue(key, out Type value))
            {
                lock (ResolvedTypes)
                {
                    if (!ResolvedTypes.TryGetValue(key, out value))
                    {
                        ResolvedTypes.Add(key, MakeGeneric());
                    }
                }
            }

            return value;
        }

        [Benchmark]
        public object FindByDynamic()
            => GetInterface((dynamic) theState, (dynamic) theEvent);

        public Type GetInterface<TState, TEvent>(TState state, TEvent @event)
            => typeof(IApplyEvent<TState, TEvent>);

    }
}