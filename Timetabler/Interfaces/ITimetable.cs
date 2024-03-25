using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ITimetable
    {
        IReadOnlyList<CycleSlot> WeekSlots { get; }
        TimetableConstraints Constraints { get; }
        IReadOnlyList<IResource> Resources { get; }
        IReadOnlyList<IBlock> Blocks { get; }
        void CreateSlots(int days, int slotsPerDay);
    }
}
