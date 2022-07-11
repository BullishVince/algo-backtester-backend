namespace AlgoBacktesterBackend.Api.Config.Models;
public class ApiInformation {
    public string Status { get; private set; }
    public float Version { get; private set; }

    public ApiInformation(string status, float version) {
        Status = status;
        Version = version;
    }
}