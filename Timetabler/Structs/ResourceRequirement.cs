using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetabler.Structs
{
    public struct ResourceRequirement
    {
        public ResourceRequirement(string resourceTag, int quantity)
        {
            ResourceTag = resourceTag;
            Quantity = quantity;
        }

        public string ResourceTag { get; set; }
        public int Quantity { get; set; }
    }
}
