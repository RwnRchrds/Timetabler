namespace Timetabler.Structs;

public class TimetableConstraints
{
    internal TimetableConstraints()
    {
        AllowSlotsToOverflowDays = false;
        AllowConsecutiveSameEventSessions = false;
        ShuffleSlotsOnAssignment = true;
    }
    
    public bool AllowSlotsToOverflowDays { get; set; }
    public bool AllowConsecutiveSameEventSessions { get; set; }
    public bool ShuffleSlotsOnAssignment { get; set; }
}