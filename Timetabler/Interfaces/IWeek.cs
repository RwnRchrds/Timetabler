using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IWeek : ISessionOwner
    {
        ITimetable Timetable { get; }
        string Name { get; }
        IBlock[] Blocks { get; }
        IResource[] Resources { get; }

        IBlock AddBlock(string name, int numberOfSlots);
        void RemoveBlock(string name);
        IResource AddResource(string name, params string[] tags);
        void RemoveResource(string name);
        void RemoveAllResources();
        void RemoveAllBlocks();
        IResource[] GetAvailableResources(WeekSlot slot);
        IResource[] GetAvailableResources(WeekSlot[] slots);
        WeekSlot[][] GetPossibleSlotAllocations(SpreadConstraint constraint);
        WeekSlot[][] GetPossibleSlotAllocations(SpreadConstraint spreadConstraint,
            ResourceConstraint[] resourceConstraints);
        WeekSlot[][] GetPossibleSlotAllocations(SpreadConstraint constraint, IResource[] requiredResources);
        WeekSlot[] GetAvailableSlots(IResource[] resources);
        WeekSlot[] GetAvailableSlots(IResource resource);
        ISession[] GetResourceAllocations(IResource resource, WeekSlot[] slots);
        ISession[] GetResourceAllocations(IResource resource);
        bool ResourceHasConflict(IResource resource, WeekSlot slot);
        bool ResourceHasCapacity(IResource resource);
        bool Validate(out string validationError);
        IWeek Clone(ITimetable timetable);
    }
}
