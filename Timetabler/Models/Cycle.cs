using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Cycle : ICycle
    {
        private readonly ICollection<CycleSlot> _reservations;

        public Cycle(int daysPerWeek, int slotsPerDay)
        {
            DaysPerWeek = daysPerWeek;
            SlotsPerDay = slotsPerDay;
            _reservations = new HashSet<CycleSlot>();
        }

        public void ReserveSlot(int slot)
        {
            ReserveSlot(slot, Enumerable.Range(0, DaysPerWeek).ToArray());
        }

        public void ReserveSlot(int slot, params int[] days)
        {
            if (slot < 0 || slot >= SlotsPerDay)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "The specified slot does not exist.");
            }

            foreach (var day in days)
            {
                _reservations.Add(new CycleSlot(day, slot));
            }
        }

        public int DaysPerWeek { get; set; }
        public int SlotsPerDay { get; set; }

        public CycleSlot[] Slots
        {
            get
            {
                var slots = new List<CycleSlot>();

                for (int i = 0; i < DaysPerWeek; i++)
                {
                    for (int j = 0; j < SlotsPerDay; j++)
                    {
                        slots.Add(new CycleSlot(i, j));
                    }
                }

                return slots.ToArray();
            }
        }
        public CycleSlot[] Reservations => _reservations.ToArray();
    }
}
