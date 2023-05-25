using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Interfaces
{
    public interface IResource
    {
        ICollection<string> Tags { get; }
        string Name { get; }
    }
}
