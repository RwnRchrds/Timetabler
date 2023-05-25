﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ICycle
    {
        int DaysPerWeek { get; }
        int SlotsPerDay { get; }
        Slot[] Reservations { get; }
        Slot[] Slots { get; }
    }
}