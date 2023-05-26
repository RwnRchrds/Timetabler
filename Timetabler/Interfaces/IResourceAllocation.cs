namespace Timetabler.Interfaces
{
    public interface IResourceAllocation
    {
        IResource Resource { get; set; }
        bool Locked { get; set; }
    }
}
