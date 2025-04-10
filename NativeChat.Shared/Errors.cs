namespace NativeChat;

public class BaseError<T>
{
    public T Errors { get; set; }

    public BaseError(T message)
    {
        Errors = message;
    }
}

public class InternalServerError<T> : BaseError<T>
{
    public InternalServerError(T errors) : base(errors) { }
}