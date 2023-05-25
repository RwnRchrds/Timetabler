using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Interfaces
{
    public interface IBlock
    {
        string Name { get; }
        ICollection<IEventGroup> EventGroups { get; }
        bool Validate();
    }
}
