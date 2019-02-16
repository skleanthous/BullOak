namespace BullOak.Repositories.Appliers
{
    public interface IConstructScopedResolver
    {
        IResolveDependencies GetScopedResolver();
    }
}