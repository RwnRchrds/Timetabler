using Timetabler.Exceptions;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Timetable : ITimetable
    {
        private readonly ICollection<IBreak> _breaks;
        private readonly ICollection<IWeek> _weeks;
        private readonly ICollection<WeekSlot> _reservations;

        public Timetable(int slotsPerDay, int daysPerWeek, int weeksPerCycle)
        {
            Cycle = new Cycle(slotsPerDay, daysPerWeek, weeksPerCycle);
            FirstDayOfWeek = DayOfWeek.Monday;
            _breaks = new HashSet<IBreak>();
            _weeks = new HashSet<IWeek>();
            _reservations = new HashSet<WeekSlot>();
        }

        public void RemoveAllWeeks()
        {
            _weeks.Clear();
        }

        public IBreak AddBreak(string name, WeekSlot start, WeekSlot end)
        {
            var timetableBreak = _breaks.FirstOrDefault(x => x.Name == name);

            if (timetableBreak == null)
            {
                timetableBreak = new Break(name, start, end);
                _breaks.Add(timetableBreak);
            }

            return timetableBreak;
        }

        public void RemoveBreak(string name)
        {
            var timetableBreak = _breaks.FirstOrDefault(x => x.Name == name);

            if (timetableBreak != null)
            {
                _breaks.Remove(timetableBreak);
            }
        }

        public void RemoveAllBreaks()
        {
            _breaks.Clear();
        }

        /// <summary>
        /// Generates a solution for each week from the blocks that have been specified. A return value indicates whether a solution was able to be found.
        /// </summary>
        /// <returns></returns>
        public bool TrySolve(ISolver solver)
        {
            return solver.TrySolve(this);
        }

        public IWeek AddWeek(string name)
        {
            var week = _weeks.FirstOrDefault(w => w.Name == name);

            if (week == null)
            {
                week = new Week(this, name);
                _weeks.Add(week);
            }

            return week;
        }

        public void AddWeek(IWeek week)
        {
            var existingWeek = _weeks.FirstOrDefault(w => w.Name == week.Name);

            if (existingWeek != null)
            {
                _weeks.Add(week);
            }
        }

        public void PopulateWeeks()
        {
            var weeksToCreate = Cycle.WeeksPerCycle - _weeks.Count;

            if (weeksToCreate > 0)
            {
                for (int i = 0; i < weeksToCreate; i++)
                {
                    AddWeek($"Week {i + 1}");
                }
            }
        }

        public void RemoveWeek(string name)
        {
            var week = _weeks.FirstOrDefault(w => w.Name == name);

            if (week != null)
            {
                _weeks.Remove(week);
            }
        }

        public bool Validate(out string validationError)
        {
            validationError = "";

            foreach (var week in Weeks)
            {
                if (!week.Validate(out validationError))
                {
                    return false;
                }
            }

            return true;
        }

        public void ReserveSlot(int slot)
        {
            ReserveSlot(slot, Enumerable.Range(0, Cycle.DaysPerWeek).ToArray());
        }

        public void ReserveSlot(int slot, params int[] days)
        {
            if (slot < 0 || slot >= Cycle.SlotsPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "The specified slot does not exist.");
            }

            foreach (var day in days)
            {
                _reservations.Add(new WeekSlot(day, slot));
            }
        }

        public bool AreSlotsConsecutive(WeekSlot a, WeekSlot b)
        {
            var cycleSlots = Slots.OrderBy(s => s.Day).ThenBy(s => s.Slot).ToArray();

            if (cycleSlots.Contains(a) && cycleSlots.Contains(b))
            {
                return Math.Abs(Array.IndexOf(cycleSlots, a) - Array.IndexOf(cycleSlots, b)) == 1;
            }

            throw new CycleException("The timetable cycle does not contain the slots you specified.");
        }

        public Cycle Cycle { get; }
        public DayOfWeek FirstDayOfWeek { get; set; }
        public bool AllowSlotOverflow { get; set; }

        public WeekSlot[] Slots
        {
            get
            {
                var slots = new List<WeekSlot>();

                for (int i = 0; i < Cycle.DaysPerWeek; i++)
                {
                    for (int j = 0; j < Cycle.SlotsPerDay; j++)
                    {
                        slots.Add(new WeekSlot(i, j));
                    }
                }

                return slots.ToArray();
            }
        }

        public IBreak[] Breaks => _breaks.ToArray();

        public WeekSlot[] UnreservedSlots => Slots.Where(s => !Reservations.Contains(s)).ToArray();

        public WeekSlot[] GetSlots(int index)
        {
            return Slots.Where(s => s.Slot == index).ToArray();
        }

        /// <summary>
        /// The weeks that provide a structure for the timetable.
        /// </summary>
        public IWeek[] Weeks => _weeks.ToArray();

        public WeekSlot[] Reservations => _reservations.ToArray();
        public WeekSlot GetSlot(int day, int index)
        {
            return Slots.FirstOrDefault(s => s.Day == day && s.Slot == index);
        }

        internal Dictionary<DayOfWeek, int> GetDayOfWeekMapping()
        {
            var mapping = new Dictionary<DayOfWeek, int>();

            DayOfWeek day = FirstDayOfWeek;

            for (int i = 0; i < Cycle.DaysPerWeek; i++)
            {
                mapping.Add(day, i);

                if (day == DayOfWeek.Sunday)
                {
                    day = DayOfWeek.Monday;
                }
                else
                {
                    day++;
                }
            }

            return mapping;
        }

        public WeekSlot GetSlot(DayOfWeek day, int index)
        {
            var mapping = GetDayOfWeekMapping();

            var dayExists = mapping.TryGetValue(day, out var dayIndex);

            if (dayExists)
            {
                return GetSlot(dayIndex, index);
            }

            throw new CycleException("The day you specified does not exist in the current timetable cycle.");
        }
    }
}
