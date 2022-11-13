namespace DAL.Infrastructure;

public class DataConflictException : Exception
{
    public DataConflictException(string message) : base(message) { }
}
