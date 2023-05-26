using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IBreak
    {
        string Name { get; }
        WeekSlot Start { get; }
        WeekSlot End { get; }
    }
}
