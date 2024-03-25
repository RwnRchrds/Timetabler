using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Occurrence : IOccurrence
    {
        private ICollection<ResourceAllocation> _resourceAllocations;

        public CycleSlot[] SlotAllocations { get; }
        public IReadOnlyList<ResourceAllocation> ResourceAllocations => _resourceAllocations.ToArray();

        public Occurrence(CycleSlot[] slotAllocations)
        {
            SlotAllocations = slotAllocations;
        }
    }
}
