using Timetabler.Extensions;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Solvers
{
    public class DefaultSolver : ISolver
    {
        public bool TrySolve(ITimetable timetable)
        {

            bool success = false;
            
            foreach (var week in timetable.Weeks)
            {
                foreach (var block in week.Blocks)
                {
                    success = TryAllocateResources(block);
                    success = TryAllocateSlots(block);
                }
            }

            if (!success)
            {
                // Discard failed timetable changes
                ClearUnlocked(timetable);
            }

            return success;
        }

        private void ClearUnlocked(ITimetable timetable)
        {
            foreach (var week in timetable.Weeks)
            {
                foreach (var block in week.Blocks)
                {
                    foreach (var eventGroup in block.EventGroups)
                    {
                        foreach (var groupEvent in eventGroup.Events)
                        {
                            groupEvent.RemoveUnlockedSessions();
                            groupEvent.DeallocateUnlockedResources();
                        }
                        
                        eventGroup.DeallocateUnlockedResources();
                    }
                    
                    block.DeallocateUnlockedResources();
                }
            }
        }

        private bool TryAllocateSlots(IBlock block)
        {
            var success = true;

            var slotConstraints = block.EventGroups.SelectMany(g => g.Events.Select(e => e.SpreadConstraint)).Distinct()
                .ToArray();

            return success;
        }

        private bool TryAllocateResources(IBlock block)
        {
            var success = true;
            
            foreach (var eventGroup in block.EventGroups)
            {
                foreach (var groupEvent in eventGroup.Events)
                {
                    
                }
            }

            return success;
        }
    }
}