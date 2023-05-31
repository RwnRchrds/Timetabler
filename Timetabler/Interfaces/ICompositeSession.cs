using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ICompositeSession : ISession
    {
        ISlotAllocation[] SlotAllocations { get; }
        void AllocateSlot(WeekSlot slot, bool locked);
        void AllocateSlots(WeekSlot[] slots, bool locked);
        void DeallocateUnlockedSlots();
        void DeallocateAllSlots();
        ICompositeSession Clone(IEvent timetableEvent);
    }
}
