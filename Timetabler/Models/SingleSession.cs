using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class SingleSession : Session, ISingleSession
    {
        public SingleSession(IEvent timetableEvent, Guid id, WeekSlot slot, bool locked) : base(timetableEvent, id, locked)
        {
            SlotAllocation = new SlotAllocation(slot, false);
        }

        public ISlotAllocation SlotAllocation { get; private set; }

        public void ChangeSlot(WeekSlot slot, bool locked)
        {
            SlotAllocation = new SlotAllocation(slot, locked);
        }

        public ISingleSession Clone(IEvent timetableEvent)
        {
            var session = new SingleSession(timetableEvent, Id, SlotAllocation.WeekSlot, Locked)
            {
                SlotAllocation = SlotAllocation
            };

            return session;
        }
    }
}
