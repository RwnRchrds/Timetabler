using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Structs
{
    public struct CycleSlot : IEquatable<CycleSlot>
    {
        public CycleSlot(int day, int slot)
        {
            Day = day;
            Slot = slot;
        }

        public int Day;
        public int Slot;

        public bool Equals(CycleSlot other)
        {
            return Day == other.Day && Slot == other.Slot;
        }

        public override bool Equals(object? obj)
        {
            return obj is CycleSlot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Slot);
        }
    }
}
