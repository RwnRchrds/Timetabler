using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.ConstraintSolver;
using Timetabler.Exceptions;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Timetable : ITimetable
    {
        private ICollection<CycleSlot> _weekSlots;
        private ICollection<IResource> _resources;
        private ICollection<IBlock> _blocks;

        public Timetable()
        {
            _weekSlots = new List<CycleSlot>();
            _resources = new List<IResource>();
            _blocks = new List<IBlock>();
        }

        public TimetableConstraints Constraints { get; } = new();
        public IReadOnlyList<CycleSlot> WeekSlots => _weekSlots.ToArray();
        public IReadOnlyList<IResource> Resources => _resources.ToArray();
        public IReadOnlyList<IBlock> Blocks => _blocks.ToArray();

        public CycleSlot AddSlot(string name, int day, int period)
        {
            var slot = new CycleSlot(name, day, period);

            if (!WeekSlots.Any(s => s.Day == slot.Day && s.Period == slot.Period))
            {
                _weekSlots.Add(slot);
            }

            return slot;
        }

        public void CreateSlots(int days, int slotsPerDay)
        {
            for (var i = 0; i < days; i++)
            {
                for (var j = 0; j < slotsPerDay; j++)
                {
                    _weekSlots.Add(new CycleSlot(i, j));
                }
            }
        }

        public IBlock AddBlock(string name)
        {
            if (Blocks.Any(b => b.Name == name))
            {
                throw new TimetableException($"A block with name '{name}' already exists.");
            }

            var block = new Block(name);
            _blocks.Add(block);
            return block;
        }

        public IResource AddResource(string name, params string[] tags)
        {
            if (Resources.Any(r => r.Name == name))
            {
                throw new TimetableException($"A resource with name '{name}' already exists.");
            }

            var resource = new Resource(name, tags);
            _resources.Add(resource);
            return resource;
        }
    }
}
