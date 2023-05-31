using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public abstract class Session : ResourceOwner, ISession
    { 
        public Session(IEvent timetableEvent, Guid id, bool locked)
        {
            Id = id;
            Event = timetableEvent;
            Locked = locked;
        }

        public Guid Id { get; }
        public bool Locked { get; }
        public IEvent Event { get; }

        public ISlotAllocation[] GetSlotAllocations()
        {
            if (this is SingleSession single)
            {
                return new[] { single.SlotAllocation };
            }
            
            if (this is CompositeSession composite)
            {
                return composite.SlotAllocations;
            }

            return Array.Empty<ISlotAllocation>();
        }

        public IResourceAllocation[] AllResourceAllocations =>
            ResourceAllocations.Union(Event.AllResourceAllocations).ToArray();
    }
}
