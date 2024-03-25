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
        public string Name { get; set; }
        SpreadConstraint SpreadConstraint { get; }
        public IReadOnlyList<ResourceConstraint> ResourceConstraints { get; }
        public IReadOnlyList<ResourceConstraint> OccurrenceResourceConstraint { get; }
        public IReadOnlyList<ResourceAllocation> ResourceAllocations { get; }
        public IReadOnlyList<IOccurrence> Occurrences { get; }
    }
}
