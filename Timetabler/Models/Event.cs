using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Event : ResourceOwner, IEvent
    {
        private readonly ICollection<ResourceConstraint> _sessionResourceConstraints;
        private readonly ICollection<ISession> _sessions;

        public Event(IEventGroup eventGroup, string name, int duration, int quantity)
        {
            EventGroup = eventGroup;
            Name = name;
            SpreadConstraint = new SpreadConstraint(duration, quantity);
            _sessionResourceConstraints = new HashSet<ResourceConstraint>();
            _sessions = new HashSet<ISession>();
        }

        public IEventGroup EventGroup { get; }
        public string Name { get; set; }
        public SpreadConstraint SpreadConstraint { get; set; }
        public ResourceConstraint[] SessionResourceConstraints => _sessionResourceConstraints.ToArray();
        public ISession[] Sessions => _sessions.ToArray();
        public void RequireSessionResource(string resourceTag, int quantity)
        {
            if (_sessionResourceConstraints.All(c => c.ResourceTag != resourceTag))
            {
                _sessionResourceConstraints.Add(new ResourceConstraint(resourceTag, quantity));
            }
        }

        public ISession AddSession(WeekSlot[] slots, bool locked)
        {
            return AddSession(Guid.NewGuid(), slots, locked);
        }

        private ISession AddSession(Guid id, WeekSlot[] slots, bool locked)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == id);

            if (session == null)
            {
                if (slots.Length > 1)
                {
                    session = new CompositeSession(this, id, slots, locked);
                }
                else
                {
                    session = new SingleSession(this, id, slots.First(), locked);
                }

                _sessions.Add(session);
                return session;
            }

            return session;
        }

        public void RemoveSession(Guid id)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == id);

            if (session != null)
            {
                _sessions.Remove(session);
            }
        }

        public void RemoveUnlockedSessions()
        {
            var unlockedSessions = _sessions.Where(s => !s.Locked).ToArray();

            foreach (var session in unlockedSessions)
            {
                _sessions.Remove(session);
            }
        }

        public void RemoveAllSessions()
        {
            _sessions.Clear();
        }

        public bool Validate(out string validationError)
        {
            validationError = "";

            if (Sessions.Length != SpreadConstraint.Quantity * SpreadConstraint.Duration)
            {
                validationError =
                    $"Event {Name} requires {SpreadConstraint.Quantity * SpreadConstraint.Duration} sessions, but has {Sessions.Length}.";
                return false;
            }

            // TODO: Check sessions intended as extended duration events do not overflow onto the following day id this is not allowed.
            // E.g. a double period should not start on Tue:Last and end one Wed:0

            return true;
        }

        public IEvent Clone(IEventGroup eventGroup)
        {
            var timetableEvent = new Event(eventGroup, Name, SpreadConstraint.Duration, SpreadConstraint.Quantity);

            foreach (var sessionResourceConstraint in SessionResourceConstraints)
            {
                RequireSessionResource(sessionResourceConstraint.ResourceTag, sessionResourceConstraint.Quantity);
            }

            foreach (var session in Sessions)
            {
                if (session is ISingleSession single)
                {
                    _sessions.Add(single.Clone(timetableEvent));
                }
                else if (session is ICompositeSession composite)
                {
                    _sessions.Add(composite.Clone(timetableEvent));
                }
            }

            return timetableEvent;
        }

        public ISlotAllocation[] SlotAllocations => Sessions.SelectMany(s => s.GetSlotAllocations()).ToArray();
    }
}
