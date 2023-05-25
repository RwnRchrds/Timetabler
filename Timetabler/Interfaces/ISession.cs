using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ISession
    {
        IEvent Event { get; }
        Slot Slot { get; }
        ICollection<IResource> Resources { get; }
        void Lock();
        void Unlock();
    }
}
