using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Break : IBreak
    {
        public Break(string name, WeekSlot start, WeekSlot end)
        {
            Name = name;
            Start = start;
            End = end;
        }

        public string Name { get; }
        public WeekSlot Start { get; }
        public WeekSlot End { get; }
    }
}
