using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Interfaces;
using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Tests
{
    [TestFixture]
    public class TimetableTests
    {
        [Test]
        public void GetAvailableResources_ReturnsCorrectResources()
        {
            var timetable = new Timetable();

            // 5 days per week, 8 slots per day
            var cycle = new Cycle(5, 8);

            // Add cycle to timetable
            timetable.Cycles.Add(cycle);

            // Create some timetable resources
            var mrsHoney = new Resource("Mrs Honey", "teacher", "maths-teacher");
            timetable.Resources.Add(mrsHoney);
            timetable.Resources.Add(new Resource("Mrs Trunchball", "teacher", "senior-staff"));
            timetable.Resources.Add(new Resource("Room 1", "room"));
            timetable.Resources.Add(new Resource("Room 2", "room"));

            // Create a new timetable block
            var block = new Block("Year 7");

            // Add an event group to block
            var class7a = new EventGroup("7A");
            block.EventGroups.Add(class7a);

            // Add an event to event group
            var maths7a = new Event("7A Maths", 1, 1);
            class7a.Events.Add(maths7a);

            // Assign event to a timetable slot
            var session = new Session(maths7a, new CycleSlot(2, 2));

            // Assign a resource to the scheduled event
            session.Resources.Add(mrsHoney);

            // Add a new timetable solution with the scheduled event
            timetable.Solution = new Dictionary<ICycle, IDictionary<IBlock, ISession[]>>();
            timetable.Solution.Add(cycle, new Dictionary<IBlock, ISession[]>
            {
                {block, new []{session}}
            });

            // Check that the resource does not appear in available resources in the slot where the event is scheduled
            // as it's being used in the scheduled event
            var result = timetable.GetAvailableResources(cycle, new CycleSlot(2, 2));
            Assert.That(result, Does.Not.Contain(mrsHoney));
        }
    }
}
