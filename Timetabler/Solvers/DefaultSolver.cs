using Timetabler.Extensions;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Solvers
{
    public class DefaultSolver : ISolver
    {
        public bool TrySolve(ITimetable timetable)
        {
            bool success = true;

            foreach (var week in timetable.Weeks)
            {
                var allEvents = week.Blocks.SelectMany(b => b.EventGroups).SelectMany(g => g.Events)
                    .OrderByDescending(e => e.SpreadConstraint.Duration).ToArray();

                foreach (var timetableEvent in allEvents)
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

                foreach (var block in week.Blocks)
                {
                    foreach (var eventGroup in block.EventGroups)
                    {
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
            singleA.DeallocateUnlockedResources();
            singleB.DeallocateUnlockedResources();

            singleA.ChangeSlot(allocationA.WeekSlot, false);
            singleB.ChangeSlot(allocationB.WeekSlot, false);

            singleA.AllocateResources(resourcesA, false);
            singleB.AllocateResources(resourcesB, false);
            return false;
        }

        private bool TrySwap(ICompositeSession compositeA, ICompositeSession compositeB)
        {
            var week = compositeA.Event.EventGroup.Block.Week;

            if (compositeB.Event.EventGroup.Block.Week != week)
            {
                throw new Exception("Weeks must be the same when swapping blocks.");
            }
            
            if (compositeA.SlotAllocations.Any(a => a.Locked) || compositeB.SlotAllocations.Any(b => b.Locked))
            {
                return false;
            }

            var allocationsA = compositeA.SlotAllocations.Select(a => a.WeekSlot).ToArray();
            var resourcesA = compositeA.ResourceAllocations.Select(a => a.Resource).ToArray();
            var eventResourcesA = compositeA.Event.ResourceAllocations.Select(a => a.Resource).ToArray();
            var eventGroupResourcesA =
                compositeA.Event.EventGroup.ResourceAllocations.Select(a => a.Resource).ToArray();
            var blockResourcesA =
                compositeA.Event.EventGroup.Block.ResourceAllocations.Select(a => a.Resource).ToArray();
            
            var allocationsB = compositeB.SlotAllocations.Select(a => a.WeekSlot).ToArray();
            var resourcesB = compositeB.ResourceAllocations.Select(a => a.Resource).ToArray();
            var eventResourcesB = compositeB.Event.ResourceAllocations.Select(a => a.Resource).ToArray();
            var eventGroupResourcesB = compositeB.Event.ResourceAllocations.Select(a => a.Resource).ToArray();
            var blockResourcesB = compositeB.Event.ResourceAllocations.Select(a => a.Resource).ToArray();

            compositeA.DeallocateUnlockedResources();
            compositeA.Event.DeallocateUnlockedResources();
            compositeA.Event.EventGroup.DeallocateUnlockedResources();
            compositeA.Event.EventGroup.Block.DeallocateUnlockedResources();

            compositeB.DeallocateUnlockedResources();
            compositeB.Event.DeallocateUnlockedResources();
            compositeB.Event.EventGroup.DeallocateUnlockedResources();
            compositeB.Event.EventGroup.Block.DeallocateUnlockedResources();

            compositeA.DeallocateAllSlots();
            compositeB.DeallocateAllSlots();

            compositeA.AllocateSlots(allocationsB, false);
            compositeB.AllocateSlots(allocationsA, false);

            var newResourcesA = TryAllocateResources(week, compositeA);
            var newEventResourcesA = TryAllocateResources(week, compositeA.Event);
            var newEventGroupResourcesA = TryAllocateResources(week, compositeA.Event.EventGroup);
            var newBlockResourcesA = TryAllocateResources(week, compositeA.Event.EventGroup.Block);
            
            var newResourcesB = TryAllocateResources(week, compositeB);
            var newEventResourcesB = TryAllocateResources(week, compositeB.Event);
            var newEventGroupResourcesB = TryAllocateResources(week, compositeB.Event.EventGroup);
            var newBlockResourcesB = TryAllocateResources(week, compositeB.Event.EventGroup.Block);

            if (newResourcesA && newResourcesB && compositeA.Event.Validate(out _) && compositeB.Event.Validate(out _))
            {
                return true;
            }

            // Undo swap
            compositeA.DeallocateAllSlots();
            compositeB.DeallocateAllSlots();

            compositeA.DeallocateUnlockedResources();
            compositeB.DeallocateUnlockedResources();

            compositeA.AllocateSlots(allocationsA, false);
            compositeB.AllocateSlots(allocationsB, false);

            compositeA.AllocateResources(resourcesA, false);
            compositeB.AllocateResources(resourcesB, false);
            return false;
        }

        private bool TryAllocateResources<T>(IWeek week, T owner) where T : IResourceOwner, ISessionOwner
        {
            owner.DeallocateUnlockedResources();

            var requiredResources = new List<ResourceConstraint>();

            foreach (var constraint in owner.ResourceConstraints)
            {
                var existingQuantity =
                    owner.ResourceAllocations.Count(a => a.Resource.Tags.Contains(constraint.ResourceTag));

                if (constraint.Quantity - existingQuantity > 0)
                {
                    requiredResources.Add(new ResourceConstraint(constraint.ResourceTag,
                        constraint.Quantity - existingQuantity));
                }
            }

            var availableResources =
                week.GetAvailableResources(owner.SlotAllocations.Select(a => a.WeekSlot).ToArray());

            foreach (var constraint in requiredResources)
            {
                var availableResourcesOfType =
                    availableResources.Where(r => r.Tags.Contains(constraint.ResourceTag)).ToArray();

                if (!availableResourcesOfType.Any())
                {
                    
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

            var requiredResources = new List<ResourceConstraint>();

            foreach (var constraint in session.ResourceConstraints)
            {
                var existingQuantity =
                    session.ResourceAllocations.Count(a => a.Resource.Tags.Contains(constraint.ResourceTag));

                if (constraint.Quantity - existingQuantity > 0)
                {
                    requiredResources.Add(new ResourceConstraint(constraint.ResourceTag,
                        constraint.Quantity - existingQuantity));
                }
            }

            var availableResources =
                week.GetAvailableResources(session.GetSlotAllocations().Select(a => a.WeekSlot).ToArray());

            foreach (var constraint in requiredResources)
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