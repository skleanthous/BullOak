namespace BullOak.Repositories.Appliers
{
    using System;

    public interface IResolveDependencies : IDisposable
    {
        object Resolve(Type typeToResolve);

        void Release(object toRelease);
    }
}