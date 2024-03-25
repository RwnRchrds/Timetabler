using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Exceptions;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Block : IBlock
    {
        private ICollection<CycleSlot> _slots;
        private ICollection<ResourceAllocation> _resourceAllocations;
        private ICollection<IEvent> _events;

        public string Name { get; set; }
        public IReadOnlyList<CycleSlot> Slots => _slots.ToArray();
        public IReadOnlyList<ResourceAllocation> ResourceAllocations => _resourceAllocations.ToArray();
        public IReadOnlyList<IEvent> Events => _events.ToArray();


        internal Block(string name)
        {
            Name = name;
            _slots = new List<CycleSlot>();
            _resourceAllocations = new List<ResourceAllocation>();
            _events = new List<IEvent>();
        }

        public IEvent AddEvent(string name, int duration, int quantity)
        {
            if (Events.Any(e => e.Name == name))
            {
                throw new TimetableException($"An event with name '{name}' already existing in block {Name}.");
            }

            var timetableEvent = new Event(name, duration, quantity);
            _events.Add(timetableEvent);
            return timetableEvent;
        }

        public void AllocateResource(IResource resource, bool locked)
        {
            if (ResourceAllocations.Any(ra => ra.Resource.Name == resource.Name))
            {
                throw new TimetableException($"Resource with name '{resource.Name}' is already allocated to this block.");
            }

            _resourceAllocations.Add(new ResourceAllocation(resource, locked));
        }
    }
}
