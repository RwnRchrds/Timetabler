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
        private readonly ICollection<Slot> _reservations;

        public Cycle(int daysPerWeek, int slotsPerDay)
        {
            DaysPerWeek = daysPerWeek;
            SlotsPerDay = slotsPerDay;
            _reservations = new HashSet<Slot>();
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
                _reservations.Add(new Slot(day, slot));
            }
        }

        public int DaysPerWeek { get; set; }
        public int SlotsPerDay { get; set; }

        public Slot[] Slots
        {
            get
            {
                var slots = new List<Slot>();

                for (int i = 0; i < DaysPerWeek; i++)
                {
                    for (int j = 0; j < SlotsPerDay; j++)
                    {
                        slots.Add(new Slot(i, j));
                    }
                }

                return slots.ToArray();
            }
        }
        public Slot[] Reservations => _reservations.ToArray();
    }
}
