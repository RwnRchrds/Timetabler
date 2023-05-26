using Timetabler.Extensions;
using Timetabler.Interfaces;

namespace Timetabler.Solvers
{
    public class DefaultSolver : ISolver
    {
        // TODO: Add swapping to try all possible combinations

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
                                var possibleSlots = week.GetPossibleSlotAllocations(timetableEvent.SpreadConstraint);

                                var allocations = possibleSlots.Shuffle().Take(requiredSessions);

                                foreach (var allocation in allocations)
                                {
                                    timetableEvent.AddSession(allocation, false);
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
