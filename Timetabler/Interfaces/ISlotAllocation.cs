using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ISlotAllocation
    {
        WeekSlot WeekSlot { get; }
        bool Locked { get; set; }
    }
}
