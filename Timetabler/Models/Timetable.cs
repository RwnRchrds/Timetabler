using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.Sat;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Timetable : ITimetable
    {
        public Timetable()
        {
            Cycles = new HashSet<ICycle>();
            Blocks = new HashSet<IBlock>();
            Resources = new HashSet<IResource>();
            Solution = new Dictionary<ICycle, IDictionary<IBlock, ISession[]>>();
        }

        /// <summary>
        /// Generates a solution for each cycle from the blocks that have been specified. A return value indicates whether a solution was able to be found.
        /// </summary>
        /// <returns></returns>
        public bool TryBuild()
        {
            throw new NotImplementedException();
        }

        public bool ValidateSolution()
        {
            throw new NotImplementedException();
        }

        public IResource[] GetAvailableResources(ICycle cycle, CycleSlot slot)
        {
            return Resources.Where(resource => !Solution.Any(solution => solution.Key == cycle 
                                                                         && solution.Value.Any(cycleSolution =>
                    cycleSolution.Value.Any(x => x.Resources.Contains(resource) && x.Slot.Equals(slot))))).ToArray();
        }

        /// <summary>
        /// The week cycles that provide a structure for the timetable.
        /// </summary>
        public ICollection<ICycle> Cycles { get; }

        /// <summary>
        /// A group of events to be added to the timetable.
        /// </summary>
        public ICollection<IBlock> Blocks { get; }

        /// <summary>
        /// A group of resources to be used by events on the timetable.
        /// </summary>
        public ICollection<IResource> Resources { get; }

        /// <summary>
        /// A solution for the timetable. This can be manually provided, or automatically generated using TryBuild().
        /// </summary>
        public IDictionary<ICycle, IDictionary<IBlock, ISession[]>> Solution { get; set; }
    }
}
