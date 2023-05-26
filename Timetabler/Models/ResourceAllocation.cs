using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class ResourceAllocation : IResourceAllocation
    {
        public ResourceAllocation(IResource resource, bool locked)
        {
            Resource = resource;
            Locked = locked;
        }

        public IResource Resource { get; set; }
        public bool Locked { get; set; }
    }
}
