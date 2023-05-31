namespace Timetabler.Interfaces;

public interface IResourceOwnerChild
{
    IResourceAllocation[] AllResourceAllocations { get; }
}