
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IBlock : IResourceOwner, ISessionOwner
    {
        IWeek Week { get; }
        string Name { get; set;  }
        IEventGroup[] EventGroups { get; }
        WeekSlot[][] SlotAllocations { get; }
        bool Validate(out string validationError);
        IEventGroup AddEventGroup(string name);
        void AddSlotAllocation(WeekSlot[] allocation);
        void RemoveAllSlotAllocations();
        void RemoveEventGroup(string name);
        void RemoveAllEventGroups();
        void RemoveUnlockedSessions();
        void RemoveAllSessions();
        IBlock Clone(IWeek week);
    }
}
