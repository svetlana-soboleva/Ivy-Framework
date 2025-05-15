using System.Text.Json;

namespace Ivy.Apps;

public class AppArgs
{
    internal AppArgs(string connectionId, string appId, string? argsJson, string host)
    {
        AppId = appId;
        ArgsJson = argsJson;
        ConnectionId = connectionId;
        Host = host;
    }

    public string Host { get; set; }
    
    public string AppId { get; set; }
    
    public string ConnectionId { get; set; }
    
    private string? ArgsJson { get; set; }
    
    public T? GetArgs<T>() where T : class
    {
        if(ArgsJson == null)
        {
            return null;
        }
        return JsonSerializer.Deserialize<T>(ArgsJson);
    } 
}