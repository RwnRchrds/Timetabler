namespace Timetabler.Interfaces
{
    public interface IEventGroup : IResourceOwner, IResourceOwnerChild, ISlotOwner
    {
        IBlock Block { get; }
        string Name { get; }
        IEvent[] Events { get; }
        IEvent AddEvent(string name, int duration, int quantity);
        void RemoveEvent(string name);
        void RemoveAllEvents();
        bool Validate(out string validationError);
        IEventGroup Clone(IBlock block);
    }
}
