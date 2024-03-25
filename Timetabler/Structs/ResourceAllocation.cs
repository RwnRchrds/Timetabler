using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;

namespace Timetabler.Structs
{
    public struct ResourceAllocation
    {
        public IResource Resource;
        public bool Locked;

        public ResourceAllocation(IResource resource, bool locked)
        {
            Resource = resource;
            Locked = locked;
        }
    }
}
