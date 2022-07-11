namespace AlgoBacktesterBackend.Api.Config.Messages;
public interface IResponseMessage<T> {
    public string Status { get; set; }
    public string[] Messages { get; set; }
    public T Data { get; set; }
}

public class ResponseMessage<T> : IResponseMessage<T> {
    public string Status { get; set; }
    public string[] Messages { get; set; }
    public T Data { get; set; }
    public ResponseMessage(string status, string[] messages, T data) {
        Status = status;
        Messages = messages;
        Data = data;
    }
}

///static class with constant strings used instead of an Enum in order to skip enum-conversions
public static class Status {
    public const string Success = "Success";
    public const string Warning = "Warning";
    public const string Error = "Error";
}