namespace Timetabler.Interfaces
{
    public interface ISolver
    {
        bool TrySolve(ITimetable timetable);
    }
}
