namespace ComandaX.Application.Exceptions;

public class RecordNotFoundException : Exception
{
    public RecordNotFoundException(string message) : base(message)
    {
    }

    public RecordNotFoundException(Guid id) : base($"Record with Id {id} not found.")
    {
    }
}

