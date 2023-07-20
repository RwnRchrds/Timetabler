namespace Timetabler.Structs
{
    public struct SpreadConstraint : IEquatable<SpreadConstraint>
    {
        public SpreadConstraint(int duration, int quantity)
        {
            Duration = duration;
            Quantity = quantity;
        }

        public readonly int Duration;
        public readonly int Quantity;

        public bool Equals(SpreadConstraint other)
        {
            return Duration == other.Duration && Quantity == other.Quantity;
        }

        public override bool Equals(object? obj)
        {
            return obj is SpreadConstraint other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Duration, Quantity);
        }
    }
}
