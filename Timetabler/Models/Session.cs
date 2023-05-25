using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;
using Timetabler.Structs;

namespace Timetabler.Models
{
    public class Session : ISession
    {
        private CycleSlot _slot;
        private bool _locked;

        public Session(IEvent timetableEvent, CycleSlot slot)
        {
            Resources = new HashSet<IResource>();
            Event = timetableEvent;
            Slot = slot;
        }
        public void Lock()
        {
            _locked = true;
        }
        public void Unlock()
        {
            _locked = false;
        }

        public IEvent Event { get; }

        public CycleSlot Slot
        {
            get { return _slot; }
            set
            {
                if (!_locked)
                {
                    _slot = value;
                }
            }
        }

        public ICollection<IResource> Resources { get; }
    }
}
