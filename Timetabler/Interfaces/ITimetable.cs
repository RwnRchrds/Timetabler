using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ITimetable
    {
        ICollection<ICycle> Cycles { get; }
        ICollection<IBlock> Blocks { get; }
        ICollection<IResource> Resources { get; }
        IDictionary<ICycle, IDictionary<IBlock, ISession[]>> Solution { get; }

        IResource[] GetAvailableResources(ICycle cycle, Slot slot);
        bool TryBuild();
        bool ValidateSolution();
    }
}
