using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class EventGroup : IEventGroup
    {
        public EventGroup(string name)
        {
            Name = name;
            Events = new HashSet<IEvent>();
        }

        public string Name { get; }
        public int TotalSlots => Events.Sum(e => e.Duration * e.Quantity);
        public ICollection<IEvent> Events { get; }
    }
}
