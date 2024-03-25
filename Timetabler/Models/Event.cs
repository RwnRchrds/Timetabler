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
    public class Event : IEvent
    {
        private ICollection<ResourceConstraint> _resourceConstraints;
        private ICollection<ResourceConstraint> _occurrenceResourceConstraint;
        private ICollection<ResourceAllocation> _resourceAllocations;
        private ICollection<IOccurrence> _occurrences;

        public string Name { get; set; }
        public SpreadConstraint SpreadConstraint { get; }
        public IReadOnlyList<ResourceConstraint> ResourceConstraints => _resourceConstraints.ToArray();
        public IReadOnlyList<ResourceConstraint> OccurrenceResourceConstraint => _resourceConstraints.ToArray();
        public IReadOnlyList<ResourceAllocation> ResourceAllocations => _resourceAllocations.ToArray(); 
        public IReadOnlyList<IOccurrence> Occurrences => _occurrences.ToArray();

        internal Event(string name, int duration, int quantity)
        {
            Name = name;
            SpreadConstraint = new SpreadConstraint(duration, quantity);
        }

        public IOccurrence AddOccurrence(CycleSlot[] slotAllocations)
        {
            var occurrence = new Occurrence(slotAllocations);
            _occurrences.Add(occurrence);
            return occurrence;
        }

        public void AllocateResource(IResource resource, bool locked)
        {
            if (ResourceAllocations.Any(ra => ra.Resource.Name == resource.Name))
            {
                throw new TimetableException($"Resource with name '{resource.Name}' is already allocated to this event.");
            }

            _resourceAllocations.Add(new ResourceAllocation(resource, locked));
        }
    }
}
