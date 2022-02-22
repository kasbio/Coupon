 
using System.Text.Unicode;

namespace System.Text.Json;

public static class JsonHelper
{
    public static T Deserialize<T>(string json)
    {
        var options = new JsonSerializerOptions (); 
        options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create (UnicodeRanges.All);
        return JsonSerializer.Deserialize<T>(json, options);
    }


    public static string Serialize(object value)
    {
        var options = new JsonSerializerOptions (); 
        options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create (UnicodeRanges.All);
        return JsonSerializer.Serialize(value);
    }
    
}