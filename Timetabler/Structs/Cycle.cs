namespace Timetabler.Structs
{
    public struct Cycle
    {
        public Cycle(int slotsPerDay, int daysPerWeek, int weeksPerCycle)
        {
            SlotsPerDay = slotsPerDay;
            DaysPerWeek = daysPerWeek;
            WeeksPerCycle = weeksPerCycle;
        }

        public readonly int SlotsPerDay;
        public readonly int DaysPerWeek;
        public readonly int WeeksPerCycle;
    }
}
