using System.Security.Principal;
using Timetabler.Interfaces;
using Timetabler.Models;
using Timetabler.Solvers;
using Timetabler.Tests.Models;

namespace Timetabler.Tests;

[TestFixture]
public class TimetableTests
{
    [Test]
    public void BasicTimetableTest()
    {
        var timetable = new Timetable(8, 5, 1);
        
        timetable.ReserveSlot(0);
        timetable.ReserveSlot(7);

        timetable.AddBreak("Break", 2, 3);
        timetable.AddBreak("Lunch", 4, 5);

        var week1 = timetable.AddWeek("Week 1");

        week1.AddResource("Mr Jones", "teacher");
        week1.AddResource("Mrs Smith", "teacher");
        week1.AddResource("Mrs White", "teacher");
        week1.AddResource("Mrs Newbie", "teacher");
        week1.AddResource("Ms Patrick", "teacher");
        week1.AddResource("Mr Smart", "teacher");

        var year7 = week1.AddResource("Year 7", "year-group", "year-7");
        var year8 = week1.AddResource("Year 8", "year-group", "year-8");

        year7.AllowSimultaneousSessions = true;
        year8.AllowSimultaneousSessions = true;

        week1.AddResource("7A", "reg-group", "7a");
        week1.AddResource("7B", "reg-group", "7b");
        week1.AddResource("7C", "reg-group", "7c");
        week1.AddResource("8A", "reg-group", "8a");
        week1.AddResource("8B", "reg-group", "8b");
        week1.AddResource("8C", "reg-group", "8c");
        
        AddSubjects(week1, 7);
        AddSubjects(week1, 8);

        timetable.TrySolve(new DefaultSolver());

        var isValid = week1.Validate(out var error);
    }

    private void AddSubjects(IWeek week, int yearGroup)
    {
        var yearGroupResource = week.Resources.FirstOrDefault(r => r.Name == $"Year {yearGroup}");

        var subjects = new SchoolSubject[]
        {
            new SchoolSubject("Art", 2, 0),
            new SchoolSubject("Citizenship", 1, 0),
            new SchoolSubject("Design & Technology", 0, 1),
            new SchoolSubject("Drama", 2, 0),
            new SchoolSubject("English", 3, 0),
            new SchoolSubject("Geography", 2, 0),
            new SchoolSubject("History", 2, 0),
            new SchoolSubject("ICT", 2, 0),
            new SchoolSubject("Languages", 2, 0),
            new SchoolSubject("Mathematics", 3, 0),
            new SchoolSubject("Music", 2, 0),
            new SchoolSubject("PE", 0, 1),
            new SchoolSubject("RE", 1, 0),
            new SchoolSubject("Science", 3, 0)
        };

        if (yearGroupResource != null)
        {
            foreach (var subject in subjects)
            {
                var block = week.AddBlock($"Year {yearGroup} {subject.SubjectName}");
                block.AllocateResource(yearGroupResource, true);

                var regGroups = week.Resources.Where(r => r.Name.StartsWith(yearGroup.ToString())).ToArray();

                foreach (var regGroup in regGroups)
                {
                    var eventGroup = block.AddEventGroup($"{regGroup.Name} {subject.SubjectName} group");
                    eventGroup.AllocateResource(regGroup, true);
                    if (subject.Singles > 0)
                    {
                        var subjectClass = eventGroup.AddEvent($"{regGroup.Name} {subject.SubjectName}", 1, subject.Singles);
                        subjectClass.RequireResource("teacher", 1);
                    }

                    if (subject.Doubles > 0)
                    {
                        var subjectClass = eventGroup.AddEvent($"{regGroup.Name} {subject.SubjectName}", 2, subject.Doubles);
                        subjectClass.RequireResource("teacher", 1);
                    }
                }
            }
        }
    }
}