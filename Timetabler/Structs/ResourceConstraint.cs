namespace Timetabler.Structs
{
    public struct ResourceConstraint
    {
        public ResourceConstraint(string resourceTag, int quantity)
        {
            ResourceTag = resourceTag;
            Quantity = quantity;
        }

        public readonly string ResourceTag;
        public readonly int Quantity;
    }
}
