
namespace Timetabler.Exceptions
{
    public class CycleException : Exception
    {
        public CycleException()
        {
        }

        public CycleException(string? message) : base(message)
        {
        }
    }
}
