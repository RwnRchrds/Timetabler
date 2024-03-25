
namespace Timetabler.Exceptions
{
    public class TimetableException : Exception
    {
        public TimetableException()
        {
        }

        public TimetableException(string? message) : base(message)
        {
        }
    }
}
