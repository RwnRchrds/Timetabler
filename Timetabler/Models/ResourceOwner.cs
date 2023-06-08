using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public abstract class ResourceOwner : IResourceOwner
    {
        protected readonly ICollection<ResourceConstraint> ResourceConstraintCollection;
        protected readonly ICollection<IResourceAllocation> ResourceAllocationCollection;

        public ResourceOwner()
        {
            ResourceConstraintCollection = new HashSet<ResourceConstraint>();
            ResourceAllocationCollection = new HashSet<IResourceAllocation>();
        }

        public ResourceConstraint[] ResourceConstraints => ResourceConstraintCollection.ToArray();
        public IResourceAllocation[] ResourceAllocations => ResourceAllocationCollection.ToArray();

        public void RequireResource(string resourceTag, int quantity)
        {
            if (ResourceConstraintCollection.All(c => c.ResourceTag != resourceTag))
            {
                ResourceConstraintCollection.Add(new ResourceConstraint(resourceTag, quantity));
            }
        }

        public void AllocateResource(IResource resource, bool locked)
        {
            if (ResourceAllocationCollection.All(allocation => allocation.Resource.Name != resource.Name))
            {
                ResourceAllocationCollection.Add(new ResourceAllocation(resource, locked));
            }
        }

        public void AllocateResources(IResource[] resources, bool locked)
        {
            foreach (var resource in resources)
            {
                AllocateResource(resource, locked);
            }
        }

        public void DeallocateUnlockedResources()
        {
            foreach (var resourceAllocation in ResourceAllocationCollection.Where(allocation => !allocation.Locked).ToArray())
            {
                ResourceAllocationCollection.Remove(resourceAllocation);
            }
        }

        public void DeallocateAllResources()
        {
            ResourceAllocationCollection.Clear();
        }

        public bool HasRequiredResources()
        {
            foreach (var constraint in ResourceConstraints)
            {
                if (ResourceAllocations.Count(a => a.Resource.Tags.Contains(constraint.ResourceTag)) !=
                    constraint.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Validate(string ownerName, out string validationError)
        {
            validationError = "";
            
            foreach (var constraint in ResourceConstraints)
            {
                var resourceCount = ResourceAllocations.Count(a => a.Resource.Tags.Contains(constraint.ResourceTag));
                
                if (resourceCount < constraint.Quantity)
                {
                    validationError =
                        $"Resource owner {ownerName} requires {constraint.Quantity} resource(s) with tag '{constraint.ResourceTag}', but has {resourceCount}.";
                    return false;
                }
            }

            return true;
        }
    }
}
