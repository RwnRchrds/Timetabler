namespace Timetabler.Structs
{
    public readonly struct CycleSlot : IEquatable<CycleSlot>, IComparable<CycleSlot>
    {
        public CycleSlot(string name, int day, int period)
        {
            Name = name;
            Day = day;
            Period = period;
        }

        public CycleSlot(int day, int period)
        {
            Day = day;
            Period = period;
            Name = $"Day {day}, Period {period}";
        }

        public readonly string Name;
        public readonly int Day;
        public readonly int Period;

        public bool Equals(CycleSlot other)
        {
            return CompareTo(other) == 0;
        }

        public int CompareTo(CycleSlot other)
        {
            if (other.Day > Day)
            {
                return 1;
            }

            if (other.Day < Day)
            {
                return -1;
            }

            var result = Period.CompareTo(other.Period);

            return result;
        }

        public bool LessThan(CycleSlot other)
        {
            return CompareTo(other) == -1;
        }

        public bool GreaterThan(CycleSlot other)
        {
            return CompareTo(other) == 1;
        }

        public bool LessThanOrEqualTo(CycleSlot other)
        {
            return LessThan(other) || Equals(other);
        }

        public bool GreaterThanOrEqualTo(CycleSlot other)
        {
            return GreaterThan(other) || Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is CycleSlot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Period);
        }
    }
}
