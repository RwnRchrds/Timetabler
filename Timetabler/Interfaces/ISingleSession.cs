using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ISingleSession : ISession
    {
        ISlotAllocation SlotAllocation { get; }
        void ChangeSlot(WeekSlot slot, bool locked);
        ISingleSession Clone(IEvent timetableEvent);
    }
}
