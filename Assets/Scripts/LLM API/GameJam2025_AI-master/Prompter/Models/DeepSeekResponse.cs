using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ConsoleApp;

using System;
using System.Text.Json;

public class DeepSeekResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("done_reason")]
    public string DoneReason { get; set; }

    [JsonPropertyName("context")]
    public int[] Context { get; set; }

    [JsonPropertyName("total_duration")]
    public long TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public long LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public long PromptEvalDuration { get; set; }

    [JsonPropertyName("eval_count")]
    public int EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public long EvalDuration { get; set; }

    public static DeepSeekResponse FromJsonWithThinking(string json)
    {
        return JsonSerializer.Deserialize<DeepSeekResponse>(json);
    }
    
    public static DeepSeekResponse FromJson(string json)
    {
        var response = JsonSerializer.Deserialize<DeepSeekResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Erlaubt auch andere Groß-/Kleinschreibung
        });

        if (response != null && !string.IsNullOrEmpty(response.Response))
        {
            response.Response = Regex.Replace(response.Response, "<think>\\n.*?\\n</think>\\n\\n", string.Empty, RegexOptions.Singleline);
        }

        return response;
    }

    public override string ToString()
    {
        return $"Model: {Model}\nCreated At: {CreatedAt}\nResponse: {Response}\nDone: {Done}\nDone Reason: {DoneReason}";
    }
}