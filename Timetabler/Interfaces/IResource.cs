namespace Timetabler.Interfaces
{
    public interface IResource
    {
        ICollection<string> Tags { get; }
        string Name { get; set; }
        int MaxAllocations { get; set; }
        bool AllowSimultaneousSessions { get; set; }
    }
}
