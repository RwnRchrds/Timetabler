using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class Block : ResourceOwner, IBlock
    {
        public IWeek Week { get; }

        protected readonly ICollection<IEventGroup> EventGroupCollection;

        public Block(IWeek week, string name)
        {
            Week = week;
            Name = name;
            EventGroupCollection = new HashSet<IEventGroup>();
        }

        public string Name { get; }
        public ISession[] Sessions => EventGroups.SelectMany(g => g.Events.SelectMany(e => e.Sessions)).ToArray();
        public ISlotAllocation[] SlotAllocations => EventGroups.SelectMany(g => g.SlotAllocations).ToArray();
        public IEventGroup[] EventGroups => EventGroupCollection.ToArray();

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
            var eventGroup = EventGroupCollection.FirstOrDefault(g => g.Name == name);

            if (eventGroup == null)
            {
                eventGroup = new EventGroup(this, name);

                EventGroupCollection.Add(eventGroup);
            }

            return eventGroup;
        }

        public void RemoveEventGroup(string name)
        {
            var eventGroup = EventGroupCollection.FirstOrDefault(g => g.Name == name);

            if (eventGroup != null)
            {
                EventGroupCollection.Remove(eventGroup);
            }
        }

        public void RemoveAllEventGroups()
        {
            EventGroupCollection.Clear();
        }

        public IBlock Clone(IWeek week)
        {
            var block = new Block(week, Name);

            foreach (var eventGroup in EventGroupCollection)
            {
                block.EventGroupCollection.Add(eventGroup.Clone(block));
            }

            return block;
        }
    }
}
