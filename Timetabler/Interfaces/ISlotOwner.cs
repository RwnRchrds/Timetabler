namespace Timetabler.Interfaces
{
    public interface ISlotOwner
    {
        ISlotAllocation[] SlotAllocations { get; }
    }
}
