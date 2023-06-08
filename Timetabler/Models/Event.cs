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

            var allocationCount = Sessions.Sum(s => s.GetSlotAllocations().Length);

            if (allocationCount != SpreadConstraint.Quantity * SpreadConstraint.Duration)
            {
                validationError =
                    $"Event {Name} requires {SpreadConstraint.Quantity * SpreadConstraint.Duration} slots, but has {allocationCount}.";
                return false;
            }

            foreach (var session in Sessions)
            {
                foreach (var constraint in session.ResourceConstraints)
                {
                    var resourceCount = session.ResourceAllocations.Count(a => a.Resource.Tags.Contains(constraint.ResourceTag));
                
                    if (resourceCount < constraint.Quantity)
                    {
                        validationError =
                            $"Event {Name} has a session that requires {constraint.Quantity} resources with tag {constraint.ResourceTag}, but only has {resourceCount}.";
                        return false;
                    }
                }
                
                var slotAllocations = session.GetSlotAllocations();
                
                if (slotAllocations.Length > 1)
                {
                    var allocationFirstSlot = slotAllocations[0];
                    
                    for (int i = 1; i < slotAllocations.Length; i++)
                    {
                        var allocation = slotAllocations[i];
                        var previousAllocation = slotAllocations[i - 1];

                        if (!EventGroup.Block.Week.Timetable.AreSlotsConsecutive(allocation.WeekSlot,
                                previousAllocation.WeekSlot))
                        {
                            validationError = $"Event {Name} consists of a session with non-consecutive slots.";
                            return false;
                        }

                        var breaks = EventGroup.Block.Week.Timetable.Breaks.ToList();

                        if (!EventGroup.Block.Week.Timetable.Constraints.AllowSlotsToOverflowDays)
                        {
                            // Add a break at the end of each day
                            for (int k = 0; k < EventGroup.Block.Week.Timetable.Cycle.DaysPerWeek; k++)
                            {
                                breaks.Add(new Break("end-of-day", new WeekSlot(k, EventGroup.Block.Week.Timetable.Cycle.SlotsPerDay - 1), new WeekSlot(k + 1, 0)));
                            }
                        }

                        var violatedBreak = breaks.FirstOrDefault(a =>
                            a.Start.LessThan(allocation.WeekSlot) && a.End.GreaterThan(allocationFirstSlot.WeekSlot));

                        if (violatedBreak != null)
                        {
                            validationError = $"Event {Name} session {session.Id} runs through break {violatedBreak.Name}";
                            return false;
                        }
                    }
                }
            }

            return Validate(Name, out validationError);
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

        public IResourceAllocation[] AllResourceAllocations =>
            ResourceAllocations.Union(EventGroup.AllResourceAllocations).ToArray();
    }
}
