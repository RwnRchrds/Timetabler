using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface ITimetable
    {
        Cycle Cycle { get; }
        DayOfWeek FirstDayOfWeek { get; set; }
        bool AllowSlotOverflow { get; set; }
        WeekSlot[] Slots { get; }
        IBreak[] Breaks { get; }
        WeekSlot[] UnreservedSlots { get; }
        WeekSlot[] Reservations { get; }
        WeekSlot GetSlot(int day, int index);
        WeekSlot GetSlot(DayOfWeek day, int index);
        WeekSlot[] GetSlots(int index);
        IWeek[] Weeks { get; }
        IWeek AddWeek(string name);
        void AddWeek(IWeek week);
        void PopulateWeeks();
        void RemoveWeek(string name);
        void RemoveAllWeeks();
        IBreak AddBreak(string name, WeekSlot start, WeekSlot end);
        void RemoveBreak(string name);
        void RemoveAllBreaks();
        bool TrySolve(ISolver solver);
        bool Validate(out string validationError);
        void ReserveSlot(int slot);
        void ReserveSlot(int slot, params int[] days);
        bool AreSlotsConsecutive(WeekSlot a, WeekSlot b);
    }
}
