using System.Collections.Generic;

public interface IAnalyticService
{
    public void StartSession();
    public void StartRun();
    public void LogEvent(string eventName, Dictionary<string, object> data);

    public string SessionId { get; }
    public string RunId { get; }
}
