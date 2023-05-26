using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Week : IWeek
    {
        private readonly ICollection<IResource> _resources;
        private readonly ICollection<IBlock> _blocks;

        public Week(ITimetable timetable, string name)
        {
            Name = name;
            Timetable = timetable;
            _resources = new HashSet<IResource>();
            _blocks = new HashSet<IBlock>();
        }

        public ITimetable Timetable { get; }
        public string Name { get; }

        public IResource[] Resources => _resources.ToArray();

        public IBlock[] Blocks => _blocks.ToArray();

        public IBlock AddBlock(string name)
        {
            var block = _blocks.FirstOrDefault(b => b.Name == name);

            if (block == null)
            {
                block = new Block(this, name);
                _blocks.Add(block);
            }

            return block;
        }

        public void RemoveBlock(string name)
        {
            var block = _blocks.FirstOrDefault(b => b.Name == name);

            if (block != null)
            {
                _blocks.Remove(block);
            }
        }

        public IResource AddResource(string name, params string[] tags)
        {
            var resource = _resources.FirstOrDefault(r => r.Name == name);

            if (resource == null)
            {
                resource = new Resource(name, tags);
                _resources.Add(resource);
            }

            return resource;
        }

        public void RemoveResource(string name)
        {
            var resource = _resources.FirstOrDefault(r => r.Name == name);

            if (resource != null)
            {
                _resources.Remove(resource);
            }
        }

        public void RemoveAllResources()
        {
            _resources.Clear();
        }

        public void RemoveAllBlocks()
        {
            _blocks.Clear();
        }

        public IResource[] GetAvailableResources(WeekSlot slot)
        {
            var resources = Resources.Where(r => Blocks
                .SelectMany(b => b.EventGroups.SelectMany(g => g.Events.SelectMany(e => e.Sessions)))
                .Where(s => s.GetSlotAllocations().Any(a => a.WeekSlot.Equals(slot)))
                .All(s => !s.AllResourceAllocations.Select(a => a.Resource).Contains(r))).ToArray();

            return resources;
        }

        public IResource[] GetAvailableResources(WeekSlot[] slots)
        {
            var resources = Resources.Where(r => Blocks
                .SelectMany(b => b.EventGroups.SelectMany(g => g.Events.SelectMany(e => e.Sessions)))
                .Where(s => s.GetSlotAllocations().Any(a => slots.Contains(a.WeekSlot)))
                .All(s => !s.AllResourceAllocations.Select(a => a.Resource).Contains(r))).ToArray();

            return resources;
        }

        public WeekSlot[][] GetPossibleSlotAllocations(SpreadConstraint constraint)
        {
            var allocations = new List<WeekSlot[]>();

            var unreservedSlots = Timetable.UnreservedSlots.OrderBy(s => s.Day).ThenBy(s => s.Slot).ToArray();

            if (constraint.Duration == 1)
            {
                foreach (var slot in Timetable.UnreservedSlots)
                {
                    allocations.Add(new[] { slot });
                }
            }
            else
            {
                var allocation = new List<WeekSlot>();
                for (int i = 0; i < Timetable.UnreservedSlots.Length; i++)
                {
                    if (allocation.Count == constraint.Duration)
                    {
                        allocations.Add(allocation.ToArray());
                    }

                    allocation.Clear();

                    allocation.Add(unreservedSlots[i]);

                    for (int j = 0; j < constraint.Duration - 1; j++)
                    {
                        if (i + 1 + j >= unreservedSlots.Length)
                        {
                            break;
                        }

                        var allocationFirstSlot = allocation.First();
                        var previousSlot = allocation.Last();
                        var slotToInsert = unreservedSlots[i + j + 1];

                        if (!Timetable.AreSlotsConsecutive(previousSlot, slotToInsert))
                        {
                            break;
                        }

                        var breaks = Timetable.Breaks.ToList();

                        if (!Timetable.AllowSlotOverflow)
                        {
                            // Add a break at the end of each day
                            for (int k = 0; k < Timetable.Cycle.DaysPerWeek; k++)
                            {
                                breaks.Add(new Break("end-of-day", new WeekSlot(k, Timetable.Cycle.SlotsPerDay - 1), new WeekSlot(k + 1, 0)));
                            }
                        }

                        if (breaks.Any(a => a.Start.LessThan(slotToInsert) && a.End.GreaterThan(allocationFirstSlot)))
                        {
                            break;
                        }

                        allocation.Add(slotToInsert);
                    }
                }
            }

            return allocations.ToArray();
        }

        public WeekSlot[] GetAvailableSlots(IResource[] resources)
        {
            var slots = Timetable.UnreservedSlots.Where(slot => Blocks
                .SelectMany(b => b.EventGroups.SelectMany(g => g.Events).SelectMany(e => e.Sessions))
                .Where(s => s.ResourceAllocations.Any(a => resources.Contains(a.Resource)))
                .Any(s => s.GetSlotAllocations().Any(a => a.WeekSlot.Equals(slot)))).ToArray();

            return slots;
        }

        public WeekSlot[] GetAvailableSlots(IResource resource)
        {
            var slots = Timetable.UnreservedSlots.Where(slot => Blocks
                .SelectMany(b => b.EventGroups.SelectMany(g => g.Events).SelectMany(e => e.Sessions))
                .Where(s => s.ResourceAllocations.Any(a => a.Resource == resource))
                .Any(s => s.GetSlotAllocations().Any(a => a.WeekSlot.Equals(slot)))).ToArray();

            return slots;
        }

        public bool HasConflict(IResource resource, WeekSlot slot)
        {
            var sessions = Blocks.SelectMany(b => b.EventGroups.SelectMany(g => g.Events.SelectMany(e => e.Sessions)))
                .Where(s => s.GetSlotAllocations().Any(a => a.WeekSlot.Equals(slot))).ToArray();

            var resourceUsage = sessions.SelectMany(s => s.AllResourceAllocations).Where(a => a.Resource == resource)
                .ToArray();

            return resourceUsage.Length > 1;
        }

        public bool Validate(out string validationError)
        {
            validationError = "";

            foreach (var resource in Resources)
            {
                foreach (var timetableSlot in Timetable.Slots)
                {
                    if (HasConflict(resource, timetableSlot))
                    {
                        validationError =
                            $"Resource {resource} has conflict at day {timetableSlot.Day} slot {timetableSlot.Slot}.";
                        return false;
                    }
                }
            }

            foreach (var block in Blocks)
            {
                if (!block.Validate(out validationError))
                {
                    return false;
                }
            }

            return true;
        }

        public IWeek Clone(ITimetable timetable)
        {
            var week = new Week(timetable, Name);

            foreach (var resource in Resources)
            {
                week._resources.Add(resource);
            }

            foreach (var block in Blocks)
            {
                week._blocks.Add(block.Clone(week));
            }

            timetable.AddWeek(week);

            return week;
        }
    }
}
