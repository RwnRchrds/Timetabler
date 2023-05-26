using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IWeek
    {
        ITimetable Timetable { get; }
        string Name { get; }
        IBlock[] Blocks { get; }
        IResource[] Resources { get; }

        IBlock AddBlock(string name);
        void RemoveBlock(string name);
        IResource AddResource(string name, params string[] tags);
        void RemoveResource(string name);
        void RemoveAllResources();
        void RemoveAllBlocks();
        IResource[] GetAvailableResources(WeekSlot slot);
        IResource[] GetAvailableResources(WeekSlot[] slots);
        WeekSlot[][] GetPossibleSlotAllocations(SpreadConstraint constraint);
        WeekSlot[] GetAvailableSlots(IResource[] resources);
        WeekSlot[] GetAvailableSlots(IResource resource);
        bool HasConflict(IResource resource, WeekSlot slot);
        bool Validate(out string validationError);
        IWeek Clone(ITimetable timetable);
    }
}
