using System;
using Newtonsoft.Json;

/// <summary>
/// Repräsentiert eine einzelne Antwortmöglichkeit, die vom LLM generiert wird.
/// </summary>
[Serializable]
public class OllamaChoice
{
    /// <summary>
    /// Der generierte Text der Antwort.
    /// </summary>
    [JsonProperty("text")]
    public string text { get; set; }
}