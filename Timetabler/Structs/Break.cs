using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Structs
{
    public struct Break
    {
        public string Name { get; set; }
        public readonly CycleSlot Start;
        public readonly CycleSlot End;

        public Break(string name, CycleSlot start, CycleSlot end)
        {
            Name = name;
            Start = start;
            End = end;
        }
    }
}
