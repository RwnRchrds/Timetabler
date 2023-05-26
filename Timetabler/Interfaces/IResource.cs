namespace Timetabler.Interfaces
{
    public interface IResource
    {
        ICollection<string> Tags { get; }
        string Name { get; }
    }
}
