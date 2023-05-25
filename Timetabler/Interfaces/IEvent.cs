using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IEvent
    {
        string Description { get; }
        int Duration { get; }
        int Quantity { get; }

        ICollection<ResourceRequirement> ResourceRequirements { get; }
    }
}
