using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class SlotAllocation : ISlotAllocation
    {
        public SlotAllocation(WeekSlot weekSlot, bool locked)
        {
            WeekSlot = weekSlot;
            Locked = locked;
        }

        public WeekSlot WeekSlot { get; }
        public bool Locked { get; set; }
    }
}
