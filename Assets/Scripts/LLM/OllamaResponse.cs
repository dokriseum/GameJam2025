using System;
using Newtonsoft.Json;

[Serializable]
public class OllamaResponse
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("choices")]
    public OllamaChoice[] choices { get; set; }

    public static OllamaResponse FromJson(string json)
    {
        return JsonConvert.DeserializeObject<OllamaResponse>(json);
    }
}
