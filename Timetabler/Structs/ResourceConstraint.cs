namespace Timetabler.Structs
{
    public struct ResourceConstraint : IEquatable<ResourceConstraint>
    {
        public ResourceConstraint(string resourceTag, int quantity)
        {
            ResourceTag = resourceTag;
            Quantity = quantity;
        }

        public readonly string ResourceTag;
        public readonly int Quantity;

        public bool Equals(ResourceConstraint other)
        {
            return ResourceTag == other.ResourceTag && Quantity == other.Quantity;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResourceConstraint other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ResourceTag, Quantity);
        }
    }
}
