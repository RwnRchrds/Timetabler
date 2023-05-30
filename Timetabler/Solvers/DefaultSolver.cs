using Timetabler.Extensions;
using Timetabler.Interfaces;

namespace Timetabler.Solvers
{
    public class DefaultSolver : ISolver
    {
        public bool TrySolve(ITimetable timetable)
        {
            bool success = true;

            foreach (var week in timetable.Weeks)
            {
                foreach (var block in week.Blocks)
                {
                    foreach (var eventGroup in block.EventGroups)
                    {
                        foreach (var timetableEvent in eventGroup.Events)
                        {
                            timetableEvent.RemoveUnlockedSessions();

                            var requiredSessions = timetableEvent.SpreadConstraint.Quantity -
                                                   timetableEvent.Sessions.Count(s =>
                                                       s.GetSlotAllocations().Length ==
                                                       timetableEvent.SpreadConstraint.Duration);

                            if (requiredSessions > 0)
                            {
                                var possibleSlots = week.GetPossibleSlotAllocations(timetableEvent.SpreadConstraint)
                                    .ToList();

                                possibleSlots.Shuffle();

                                for (int i = 0; i < requiredSessions; i++)
                                {
                                    bool solutionFound = false;
                                    
                                    foreach (var possibleSlot in possibleSlots)
                                    {
                                        var sessionId = timetableEvent.AddSession(possibleSlot, false);

                                        if (TryAllocateResources(week, timetableEvent))
                                        {
                                            solutionFound = true;
                                        }
                                        else
                                        {
                                            // TODO: Add swapping to try all possible combinations
                                            timetableEvent.RemoveSession(sessionId.Id);
                                        }
                                    }

                                    if (solutionFound)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (!TryAllocateResources(week, timetableEvent))
                            {
                                success = false;
                            }
                        }

                        if (!TryAllocateResources(week, eventGroup))
                        {
                            success = false;
                        }
                    }

                    if (!TryAllocateResources(week, block))
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        public bool TryAllocateResources<T>(IWeek week, T owner) where T : IResourceOwner, ISlotOwner
        {
            owner.DeallocateUnlockedResources();

            var availableResources =
                week.GetAvailableResources(owner.SlotAllocations.Select(a => a.WeekSlot).ToArray());

            foreach (var constraint in owner.ResourceConstraints)
            {
                var availableResourcesOfType =
                    availableResources.Where(r => r.Tags.Contains(constraint.ResourceTag)).ToArray();

                var resources = availableResourcesOfType.Shuffle().Take(constraint.Quantity);

                foreach (var resource in resources)
                {
                    owner.AllocateResource(resource, false);
                }
            }

            return owner.HasRequiredResources();
        }
    }
}
