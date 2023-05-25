using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class Block : IBlock
    {
        public Block(string name)
        {
            Name = name;
            EventGroups = new HashSet<IEventGroup>();
        }

        public string Name { get; set; }
        public ICollection<IEventGroup> EventGroups { get; }

        public bool Validate()
        {
            int expectedSlots = 0;
            var eventGroups = EventGroups.ToArray();

            for (int i = 0; i < eventGroups.Length; i++)
            {
                var eventGroup = eventGroups[i];

                if (i == 0)
                {
                    expectedSlots = eventGroup.TotalSlots;
                }

                if (eventGroup.TotalSlots != expectedSlots)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
