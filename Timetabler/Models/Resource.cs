using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class Resource : IResource
    {
        private readonly ICollection<string> _resourceTags;
        
        public string Name { get; set; }
        public IReadOnlyList<string> ResourceTags => _resourceTags.ToArray();

        public Resource(string name, params string[] tags)
        {
            Name = name;
            _resourceTags = tags;
        }
    }
}
