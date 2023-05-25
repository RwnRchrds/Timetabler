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
            int expectedPeriods = 0;
            var eventGroups = EventGroups.ToArray();

            for (int i = 0; i < eventGroups.Length; i++)
            {
                var eventGroup = eventGroups[i];

                if (i == 0)
                {
                    expectedPeriods = eventGroup.TotalPeriods;
                }

                if (eventGroup.TotalPeriods != expectedPeriods)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
