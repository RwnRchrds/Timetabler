using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Models;

namespace Timetabler.Tests
{
    [TestFixture]
    public class CycleTests
    {
        [Test]
        public void Slots_ReturnsCorrectNumberOfSlots()
        {
            var cycle = new Cycle(5, 8);

            var numberOfSlots = cycle.Slots.Length;

            Assert.That(numberOfSlots, Is.EqualTo(40));
        }
    }
}
