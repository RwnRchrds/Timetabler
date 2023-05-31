namespace Timetabler.Interfaces
{
    public interface ISession : IResourceOwner, IResourceOwnerChild
    {
        Guid Id { get; }
        bool Locked { get; }
        IEvent Event { get; }
        ISlotAllocation[] GetSlotAllocations();
    }
}
