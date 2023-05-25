using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Structs
{
    public struct Slot : IEquatable<Slot>
    {
        public Slot(int day, int period)
        {
            Day = day;
            Period = period;
        }

        public int Day;
        public int Period;

        public bool Equals(Slot other)
        {
            return Day == other.Day && Period == other.Period;
        }

        public override bool Equals(object? obj)
        {
            return obj is Slot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Period);
        }
    }
}
