using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetabler.Models;
using Timetabler.Structs;

namespace Timetabler.Tests
{
    [TestFixture]
    public class BlockTests
    {
        [Test]
        public void Validate_ReturnsTrueIfValid()
        {
            var block = new Block("Year 7 Classes");

            var class7a = new EventGroup("7a");
            var class7b = new EventGroup("7b");

            class7a.Events.Add(new Event("7a english", 1, 4));
            class7a.Events.Add(new Event("7a maths", 2, 2));

            class7b.Events.Add(new Event("7b english", 1, 4));
            class7b.Events.Add(new Event("7b maths", 1, 4));

            block.EventGroups.Add(class7a);
            block.EventGroups.Add(class7b);

            var result = block.Validate();

            Assert.That(result, Is.True);
        }

        [Test]
        public void Validate_ReturnsFalseIfInvalid()
        {
            var block = new Block("Year 7 Classes");

            var class7a = new EventGroup("7a");
            var class7b = new EventGroup("7b");

            class7a.Events.Add(new Event("7a english", 1, 4));
            class7a.Events.Add(new Event("7a maths", 2, 2));

            class7b.Events.Add(new Event("7b english", 1, 4));
            class7b.Events.Add(new Event("7b maths", 2, 4));

            block.EventGroups.Add(class7a);
            block.EventGroups.Add(class7b);

            var result = block.Validate();

            Assert.That(result, Is.False);
        }
    }
}
