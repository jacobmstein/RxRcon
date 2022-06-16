namespace RxRcon;

public class RconMessage
{
    public RconMessage(string message, int identifier, string type, string stacktrace)
    {
        Message = message;
        Identifier = identifier;
        Type = type;
        Stacktrace = stacktrace;
    }

    public string Message { get; }
    public int Identifier { get; }
    public string Type { get; }
    public string Stacktrace { get; set; }
}