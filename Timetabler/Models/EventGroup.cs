using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class EventGroup : ResourceOwner, IEventGroup
    {
        private readonly ICollection<IEvent> _events;

        public EventGroup(IBlock block, string name)
        {
            Block = block;
            Name = name;
            _events = new HashSet<IEvent>();
        }

        public IBlock Block { get; }
        public string Name { get; }

        public ISlotAllocation[] SlotAllocations =>
            Events.SelectMany(e => e.Sessions.SelectMany(s => s.GetSlotAllocations())).ToArray();

        public IEvent[] Events => _events.ToArray();

        public IEvent AddEvent(string name, int duration, int quantity)
        {
            var timetableEvent = _events.FirstOrDefault(e => e.Name == name);

            if (timetableEvent == null)
            {
                timetableEvent = new Event(this, name, duration, quantity);
                _events.Add(timetableEvent);
            }

            return timetableEvent;
        }

        public void RemoveEvent(string name)
        {
            var eventToRemove = _events.FirstOrDefault(e => e.Name == name);
            
            if (eventToRemove != null)
            {
                _events.Remove(eventToRemove);
            }
        }

        public void RemoveAllEvents()
        {
            _events.Clear();
        }

        public bool Validate(out string validationError)
        {
            validationError = "";

            foreach (var timetableEvent in Events)
            {
                if (!timetableEvent.Validate(out validationError))
                {
                    return false;
                }
            }

            return true;
        }

        public IEventGroup Clone(IBlock block)
        {
            var eventGroup = new EventGroup(block, Name);

            foreach (var timetableEvent in Events)
            {
                eventGroup._events.Add(timetableEvent.Clone(eventGroup));
            }

            return eventGroup;
        }
    }
}
