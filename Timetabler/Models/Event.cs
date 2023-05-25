using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Event : IEvent
    {
        public Event(string description, int duration, int quantity)
        {
            Description = description;
            Duration = duration;
            Quantity = quantity;
            ResourceRequirements = new HashSet<ResourceRequirement>();
        }

        public string Description { get; set; }
        public int Duration { get; set; }
        public int Quantity { get; set; }

        public ICollection<ResourceRequirement> ResourceRequirements { get; }
    }
}
