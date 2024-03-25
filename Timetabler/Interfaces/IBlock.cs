using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IBlock
    {
        string Name { get; set; }
        IReadOnlyList<CycleSlot> Slots { get; }
        IReadOnlyList<ResourceAllocation> ResourceAllocations { get; }
        IReadOnlyList<IEvent> Events { get; }
        IEvent AddEvent(string name, int duration, int quantity);
        void AllocateResource(IResource resource, bool locked);
    }
}
