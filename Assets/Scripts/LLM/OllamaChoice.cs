using System;
using Newtonsoft.Json;

[Serializable]
public class OllamaChoice
{
    [JsonProperty("text")]
    public string text { get; set; }
}