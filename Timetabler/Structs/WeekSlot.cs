namespace Timetabler.Structs
{
    public readonly struct WeekSlot : IEquatable<WeekSlot>, IComparable<WeekSlot>
    {
        public WeekSlot(int day, int slot)
        {
            Day = day;
            Slot = slot;
        }

        public readonly int Day;
        public readonly int Slot;

        public bool Equals(WeekSlot other)
        {
            return CompareTo(other) == 0;
        }

        public int CompareTo(WeekSlot other)
        {
            if (other.Day > Day)
            {
                return 1;
            }

            if (other.Day < Day)
            {
                return -1;
            }

            var result = Slot.CompareTo(other.Slot);

            return result;
        }

        public bool LessThan(WeekSlot other)
        {
            return CompareTo(other) == -1;
        }

        public bool GreaterThan(WeekSlot other)
        {
            return CompareTo(other) == 1;
        }

        public bool LessThanOrEqualTo(WeekSlot other)
        {
            return LessThan(other) || Equals(other);
        }

        public bool GreaterThanOrEqualTo(WeekSlot other)
        {
            return GreaterThan(other) || Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is WeekSlot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Slot);
        }
    }
}
