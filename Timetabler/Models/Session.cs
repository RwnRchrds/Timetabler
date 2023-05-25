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
        private Slot _slot;
        private bool _locked;

        public Session(IEvent timetableEvent, Slot slot)
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

        public Slot Slot
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
