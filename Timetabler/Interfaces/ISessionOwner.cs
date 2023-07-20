namespace Timetabler.Interfaces
{
    public interface ISessionOwner
    {
        ISession[] Sessions { get; }
    }
}
