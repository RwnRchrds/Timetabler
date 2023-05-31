using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IEvent : IResourceOwner, IResourceOwnerChild, ISlotOwner
    {
        IEventGroup EventGroup { get; }
        string Name { get; }
        SpreadConstraint SpreadConstraint { get; }
        ResourceConstraint[] SessionResourceConstraints { get; }
        ISession[] Sessions { get; }
        void RequireSessionResource(string resourceTag, int quantity);
        ISession AddSession(WeekSlot[] slots, bool locked);
        void RemoveSession(Guid id);
        void RemoveUnlockedSessions();
        void RemoveAllSessions();
        bool Validate(out string validationError);
        IEvent Clone(IEventGroup eventGroup);
    }
}
