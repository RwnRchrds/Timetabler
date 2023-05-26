namespace Timetabler.Structs
{
    public struct SpreadConstraint
    {
        public SpreadConstraint(int duration, int quantity)
        {
            Duration = duration;
            Quantity = quantity;
        }

        public readonly int Duration;
        public readonly int Quantity;
    }
}
