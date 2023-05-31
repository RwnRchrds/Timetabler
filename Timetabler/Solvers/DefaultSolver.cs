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
                        foreach (var timetableEvent in eventGroup.Events.OrderByDescending(e => e.SpreadConstraint.Duration))
                        {
                            timetableEvent.RemoveUnlockedSessions();

                            var requiredSessions = timetableEvent.SpreadConstraint.Quantity -
                                                   timetableEvent.Sessions.Count(s =>
                                                       s.GetSlotAllocations().Length ==
                                                       timetableEvent.SpreadConstraint.Duration);

                            if (requiredSessions > 0)
                            {
                                for (int i = 0; i < requiredSessions; i++)
                                {
                                    var possibleAllocations = week.GetPossibleSlotAllocations(timetableEvent.SpreadConstraint,
                                        timetableEvent.AllResourceAllocations.Select(a => a.Resource).ToArray()).ToList();

                                    if (!possibleAllocations.Any())
                                    {
                                        
                                    }

                                    if (timetable.Constraints.ShuffleSlotsOnAssignment)
                                    {
                                        possibleAllocations.Shuffle();
                                    }

                                    foreach (var allocation in possibleAllocations)
                                    {
                                        var session = timetableEvent.AddSession(allocation, false);

                                        if (TryAllocateResources(week, session))
                                        {
                                            break;
                                        }
                                        
                                        // TODO: Add swapping to try all possible combinations
                                        timetableEvent.RemoveSession(session.Id);
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

        private bool TrySwap(ISingleSession singleA, ISingleSession singleB)
        {
            if (singleA.SlotAllocation.Locked || singleB.SlotAllocation.Locked)
            {
                return false;
            }
            
            var allocationA = singleA.SlotAllocation;
            var allocationB = singleB.SlotAllocation;
            var resourcesA = singleA.ResourceAllocations.Select(a => a.Resource).ToArray();
            var resourcesB = singleB.ResourceAllocations.Select(a => a.Resource).ToArray();
            
            singleA.DeallocateUnlockedResources();
            singleB.DeallocateUnlockedResources();

            singleA.ChangeSlot(allocationB.WeekSlot, false);
            singleB.ChangeSlot(allocationA.WeekSlot, false);

            var newResourcesA = TryAllocateResources(singleA.Event.EventGroup.Block.Week, singleA);
            var newResourcesB = TryAllocateResources(singleB.Event.EventGroup.Block.Week, singleB);

            if (newResourcesA && newResourcesB && singleA.Event.Validate(out _) && singleB.Event.Validate(out _))
            {
                return true;
            }

            // Undo swap
            singleA.ChangeSlot(allocationA.WeekSlot, false);
            singleB.ChangeSlot(allocationB.WeekSlot, false);

            singleA.AllocateResources(resourcesA, false);
            singleB.AllocateResources(resourcesB, false);
            return false;
        }

        private bool TrySwap(ICompositeSession compositeA, ICompositeSession compositeB)
        {
            if (compositeA.SlotAllocations.Any(a => a.Locked) || compositeB.SlotAllocations.Any(b => b.Locked))
            {
                return false;
            }

            var allocationsA = compositeA.SlotAllocations.Select(a => a.WeekSlot).ToArray();
            var allocationsB = compositeB.SlotAllocations.Select(a => a.WeekSlot).ToArray();
            var resourcesA = compositeA.ResourceAllocations.Select(a => a.Resource).ToArray();
            var resourcesB = compositeB.ResourceAllocations.Select(a => a.Resource).ToArray();
            
            compositeA.DeallocateUnlockedResources();
            compositeB.DeallocateUnlockedResources();
            
            compositeA.DeallocateAllSlots();
            compositeB.DeallocateAllSlots();
            
            compositeA.AllocateSlots(allocationsB, false);
            compositeB.AllocateSlots(allocationsA, false);

            var newResourcesA = TryAllocateResources(compositeA.Event.EventGroup.Block.Week, compositeA);
            var newResourcesB = TryAllocateResources(compositeB.Event.EventGroup.Block.Week, compositeB);

            if (newResourcesA && newResourcesB && compositeA.Event.Validate(out _) && compositeB.Event.Validate(out _))
            {
                return true;
            }
            
            // Undo swap
            compositeA.DeallocateAllSlots();
            compositeB.DeallocateAllSlots();
            
            compositeA.AllocateSlots(allocationsA, false);
            compositeB.AllocateSlots(allocationsB, false);
            
            compositeA.AllocateResources(resourcesA, false);
            compositeB.AllocateResources(resourcesB, false);
            return false;
        }

        private bool TryAllocateResources<T>(IWeek week, T owner) where T : IResourceOwner, ISlotOwner
        {
            owner.DeallocateUnlockedResources();

            var availableResources =
                week.GetAvailableResources(owner.SlotAllocations.Select(a => a.WeekSlot).ToArray());

            foreach (var constraint in owner.ResourceConstraints)
            {
                var availableResourcesOfType =
                    availableResources.Where(r => r.Tags.Contains(constraint.ResourceTag)).ToArray();

                if (!availableResourcesOfType.Any())
                {
                    var allResourcesOfType =
                        week.Resources.Where(r => r.Tags.Contains(constraint.ResourceTag)).ToArray();

                    foreach (var resource in allResourcesOfType)
                    {
                        var conflicts = week.GetAllocations(resource,
                            owner.SlotAllocations.Select(a => a.WeekSlot).ToArray());
                    }
                }

                var resources = availableResourcesOfType.Shuffle().Take(constraint.Quantity);

                foreach (var resource in resources)
                {
                    owner.AllocateResource(resource, false);
                }
            }

            if (owner.HasRequiredResources())
            {
                return true;
            }

            return false;
        }

        private bool TryAllocateResources(IWeek week, ISession session)
        {
            session.DeallocateUnlockedResources();

            var availableResources =
                week.GetAvailableResources(session.GetSlotAllocations().Select(a => a.WeekSlot).ToArray());
            
            foreach (var constraint in session.ResourceConstraints)
            {
                var availableResourcesOfType =
                    availableResources.Where(r => r.Tags.Contains(constraint.ResourceTag)).ToArray();

                var resources = availableResourcesOfType.Shuffle().Take(constraint.Quantity);

                foreach (var resource in resources)
                {
                    session.AllocateResource(resource, false);
                }
            }

            if (session.HasRequiredResources())
            {
                return true;
            }

            return false;
        }
    }
}
