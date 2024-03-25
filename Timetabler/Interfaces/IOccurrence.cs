using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IOccurrence
    {
        CycleSlot[] SlotAllocations { get; }
        IReadOnlyList<ResourceAllocation> ResourceAllocations { get; }
    }
}
