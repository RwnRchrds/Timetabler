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
    public void Test()
    {
        var path = @"/folder/sub/document.txt";

        var fileName = Path.GetFileName(path);

        var dir = path.Replace(fileName, "");
    }
    
    [Test]
    public void BasicTimetableTest()
    {
        var timetable = new Timetable(8, 5, 1);
        
        timetable.ReserveSlotOnAllDays(0);
        timetable.ReserveSlotOnAllDays(7);

        timetable.AddBreak("Break", 2, 3);
        timetable.AddBreak("Lunch", 4, 5);

        var week1 = timetable.AddWeek("Week 1");

        week1.AddResource("Mrs Jenwood", "art-teacher");
        week1.AddResource("Ms Arindell", "art-teacher");
        week1.AddResource("Ms Gauld", "citizenship-teacher");
        week1.AddResource("Mr Simpson", "citizenship-teacher");
        week1.AddResource("Mr Parsons", "dt-teacher");
        week1.AddResource("Mr Alban", "dt-teacher");
        week1.AddResource("Mr Bridgett", "drama-teacher");
        week1.AddResource("Miss Swan", "drama-teacher");
        week1.AddResource("Mr Morrish", "english-teacher");
        week1.AddResource("Mrs Boulnois", "english-teacher");
        week1.AddResource("Miss Raeburn", "geography-teacher");
        week1.AddResource("Ms Bassin", "geography-teacher");
        week1.AddResource("Mrs Goodman", "history-teacher");
        week1.AddResource("Mr Matthews", "history-teacher");
        week1.AddResource("Mr Richards", "ict-teacher");
        week1.AddResource("Mr Bate", "ict-teacher");
        week1.AddResource("Miss Symonds", "languages-teacher");
        week1.AddResource("Ms Field", "languages-teacher");
        week1.AddResource("Miss French", "mathematics-teacher");
        week1.AddResource("Mrs Pimlott", "mathematics-teacher");
        week1.AddResource("Mr Honey", "music-teacher");
        week1.AddResource("Mrs Ridgeon", "music-teacher");
        week1.AddResource("Mrs Harwood", "pe-teacher");
        week1.AddResource("Mr Marker", "pe-teacher");
        week1.AddResource("Mr Angafor", "re-teacher");
        week1.AddResource("Mr Browne", "re-teacher");
        week1.AddResource("Mrs Milner", "science-teacher");
        week1.AddResource("Ms Bell", "science-teacher");

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

        var solver = new DefaultSolver();

        solver.TrySolve(timetable);

        var isValid = week1.Validate(out var error);
    }

    private void AddSubjects(IWeek week, int yearGroup)
    {
        var yearGroupResource = week.Resources.FirstOrDefault(r => r.Name == $"Year {yearGroup}");

        var subjects = new SchoolSubject[]
        {
            new SchoolSubject("Art", 2, 0),
            new SchoolSubject("Citizenship", 1, 0),
            new SchoolSubject("DT", 0, 1),
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
                var block = week.AddBlock($"Year {yearGroup} {subject.SubjectName}", 30);
                block.AllocateResource(yearGroupResource, true);

                var regGroups = week.Resources.Where(r => r.Name.StartsWith(yearGroup.ToString())).ToArray();

                foreach (var regGroup in regGroups)
                {
                    var eventGroup = block.AddEventGroup($"{regGroup.Name} {subject.SubjectName} group");
                    eventGroup.AllocateResource(regGroup, true);
                    if (subject.Singles > 0)
                    {
                        var subjectClass = eventGroup.AddEvent($"{regGroup.Name} {subject.SubjectName}", 1, subject.Singles);
                        subjectClass.RequireResource($"{subject.SubjectName.ToLower()}-teacher", 1);
                    }

                    if (subject.Doubles > 0)
                    {
                        var subjectClass = eventGroup.AddEvent($"{regGroup.Name} {subject.SubjectName}", 2, subject.Doubles);
                        subjectClass.RequireResource($"{subject.SubjectName.ToLower()}-teacher", 1);
                    }
                }
            }
        }
    }
}