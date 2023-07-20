using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Block : ResourceOwner, IBlock
    {
        public IWeek Week { get; }

        private readonly ICollection<IEventGroup> _eventGroupCollection;
        private readonly ICollection<WeekSlot[]> _slotAllocations;

        public Block(IWeek week, string name)
        {
            Week = week;
            Name = name;
            _eventGroupCollection = new HashSet<IEventGroup>();
            _slotAllocations = new HashSet<WeekSlot[]>();
        }

        public string Name { get; set; }
        public ISession[] Sessions => EventGroups.SelectMany(g => g.Events.SelectMany(e => e.Sessions)).ToArray();
        public IEventGroup[] EventGroups => _eventGroupCollection.ToArray();
        public WeekSlot[][] SlotAllocations => _slotAllocations.ToArray();

        public bool Validate(out string validationError)
        {
            validationError = "";

            foreach (var eventGroup in EventGroups)
            {
                if (!eventGroup.Validate(out validationError))
                {
                    return false;
                }
            }

            return Validate(Name, out validationError);
        }

        public IEventGroup AddEventGroup(string name)
        {
            var eventGroup = _eventGroupCollection.FirstOrDefault(g => g.Name == name);

            if (eventGroup == null)
            {
                eventGroup = new EventGroup(this, name);

                _eventGroupCollection.Add(eventGroup);
            }

            return eventGroup;
        }

        public void AddSlotAllocation(WeekSlot[] allocation)
        {
            _slotAllocations.Add(allocation);
        }

        public void RemoveAllSlotAllocations()
        {
            _slotAllocations.Clear();
        }

        public void RemoveEventGroup(string name)
        {
            var eventGroup = _eventGroupCollection.FirstOrDefault(g => g.Name == name);

            if (eventGroup != null)
            {
                _eventGroupCollection.Remove(eventGroup);
            }
        }

        public void RemoveAllEventGroups()
        {
            _eventGroupCollection.Clear();
        }
        
        public void RemoveUnlockedSessions()
        {
            foreach (var eventGroup in EventGroups)
            {
                foreach (var groupEvent in eventGroup.Events)
                {
                    groupEvent.RemoveUnlockedSessions();
                }
            }
        }
        
        public void RemoveAllSessions()
        {
            foreach (var eventGroup in EventGroups)
            {
                foreach (var groupEvent in eventGroup.Events)
                {
                    groupEvent.RemoveAllSessions();
                }
            }
        }

        public IBlock Clone(IWeek week)
        {
            var block = new Block(week, Name);

            foreach (var eventGroup in _eventGroupCollection)
            {
                block._eventGroupCollection.Add(eventGroup.Clone(block));
            }

            return block;
        }
    }
}
