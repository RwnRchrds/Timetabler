using Timetabler.Interfaces;

namespace Timetabler.Models
{
    public class Resource : IResource
    {
        public Resource(string name, params string[] tags)
        {
            Name = name;
            Tags = new List<string>();
            MaxAllocations = 0;
            AllowSimultaneousSessions = false;

            AddTags(tags);
        }

        public ICollection<string> Tags { get; }
        public string Name { get; set; }
        public int MaxAllocations { get; set; }
        public bool AllowSimultaneousSessions { get; set; }

        public void AddTags(params string[] tags)
        {
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }
    }
}
