
namespace Timetabler.Interfaces
{
    public interface IBlock : IResourceOwner, ISlotOwner
    {
        IWeek Week { get; }
        string Name { get; }
        IEventGroup[] EventGroups { get; }
        bool Validate(out string validationError);
        IEventGroup AddEventGroup(string name);
        void RemoveEventGroup(string name);
        void RemoveAllEventGroups();
        IBlock Clone(IWeek week);
    }
}
