namespace Timetabler.Interfaces
{
    public interface ISessionOwner
    {
        ISession[] Sessions { get; }
        ISlotAllocation[] SlotAllocations { get; }
    }
}
