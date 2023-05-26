﻿using Timetabler.Structs;

namespace Timetabler.Interfaces
{
    public interface IResourceOwner
    {
        ResourceConstraint[] ResourceConstraints { get; }
        IResourceAllocation[] ResourceAllocations { get; }
        void RequireResource(string resourceTag, int quantity);
        void AllocateResource(IResource resource, bool locked);
        void DeallocateUnlockedResources();
        void DeallocateAllResources();
        bool HasRequiredResources();
    }
}
