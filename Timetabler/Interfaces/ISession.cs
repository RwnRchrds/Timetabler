namespace Timetabler.Interfaces
{
    public interface ISession : IResourceOwner
    {
        Guid Id { get; }
        bool Locked { get; }
        IEvent Event { get; }
        ISlotAllocation[] GetSlotAllocations();
        IResourceAllocation[] AllResourceAllocations { get; }
    }
}
