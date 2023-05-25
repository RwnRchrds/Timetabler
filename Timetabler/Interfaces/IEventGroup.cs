using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Interfaces
{
    public interface IEventGroup
    {
        string Name { get; }
        int TotalPeriods { get; }
        ICollection<IEvent> Events { get; }
    }
}
