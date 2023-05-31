namespace Timetabler.Tests.Models;

public class SchoolSubject
{
    public SchoolSubject(string subjectName, int singles, int doubles)
    {
        SubjectName = subjectName;
        Singles = singles;
        Doubles = doubles;
    }
    
    public string SubjectName { get; set; }
    public int Singles { get; set; }
    public int Doubles { get; set; }
}