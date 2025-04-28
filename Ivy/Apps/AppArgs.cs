using System.Text.Json;

namespace Ivy.Apps;

public class AppArgs
{
    internal AppArgs(string appId, string? argsJson)
    {
        AppId = appId;
        ArgsJson = argsJson;
    }

    public string AppId { get; set; }
    
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