using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class CompositeSession : Session, ICompositeSession
    {
        private readonly ICollection<ISlotAllocation> _slotAllocations;

        public CompositeSession(IEvent timetableEvent, Guid id, WeekSlot[] slots, bool locked) : base(timetableEvent, id, locked)
        {
            _slotAllocations = new HashSet<ISlotAllocation>();

            foreach (var slot in slots)
            {
                _slotAllocations.Add(new SlotAllocation(slot, false));
            }
        }

        public ISlotAllocation[] SlotAllocations => _slotAllocations.ToArray();

        public void AllocateSlot(WeekSlot slot, bool locked)
        {
            if (!_slotAllocations.Any(a => a.WeekSlot.Day == slot.Day && a.WeekSlot.Slot == slot.Slot))
            {
                _slotAllocations.Add(new SlotAllocation(slot, locked));
            }
        }

        public void AllocateSlots(WeekSlot[] slots, bool locked)
        {
            foreach (var slot in slots)
            {
                AllocateSlot(slot, locked);
            }
        }

        public void DeallocateUnlockedSlots()
        {
            var unlockedSlots = _slotAllocations.Where(a => !a.Locked).ToArray();

            foreach (var slot in unlockedSlots)
            {
                _slotAllocations.Remove(slot);
            }
        }

        public void DeallocateAllSlots()
        {
            _slotAllocations.Clear();
        }

        public ICompositeSession Clone(IEvent timetableEvent)
        {
            var session = new CompositeSession(timetableEvent, Id, SlotAllocations.Select(a => a.WeekSlot).ToArray(), Locked);

            foreach (var slotAllocation in SlotAllocations)
            {
                session.AllocateSlot(slotAllocation.WeekSlot, slotAllocation.Locked);
            }

            return session;
        }
    }
}
