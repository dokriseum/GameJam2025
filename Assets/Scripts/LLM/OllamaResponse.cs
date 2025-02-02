using System;
using Newtonsoft.Json;

/// <summary>
/// Repräsentiert die Antwort des LLM, inklusive aller relevanten Metadaten und generierten Texten.
/// </summary>
[Serializable]
public class OllamaResponse
{
    /// <summary>
    /// Das verwendete Modell.
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; set; }

    /// <summary>
    /// Das Erstellungsdatum der Antwort.
    /// </summary>
    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    /// <summary>
    /// Der gesamte generierte Text.
    /// </summary>
    [JsonProperty("response")]
    public string Response { get; set; }

    /// <summary>
    /// Gibt an, ob die Antwort vollständig generiert wurde.
    /// </summary>
    [JsonProperty("done")]
    public bool Done { get; set; }

    /// <summary>
    /// Grund, warum die Antwort beendet wurde.
    /// </summary>
    [JsonProperty("done_reason")]
    public string DoneReason { get; set; }

    /// <summary>
    /// Kontext-IDs, die für die Anfrage relevant sind.
    /// </summary>
    [JsonProperty("context")]
    public int[] Context { get; set; }

    /// <summary>
    /// Gesamtdauer der Anfrage in Millisekunden.
    /// </summary>
    [JsonProperty("total_duration")]
    public long TotalDuration { get; set; }

    /// <summary>
    /// Dauer des Ladens der Antwort.
    /// </summary>
    [JsonProperty("load_duration")]
    public long LoadDuration { get; set; }

    /// <summary>
    /// Anzahl der Token, die für den Prompt verwendet wurden.
    /// </summary>
    [JsonProperty("prompt_eval_count")]
    public int PromptEvalCount { get; set; }

    /// <summary>
    /// Dauer der Verarbeitung des Prompts.
    /// </summary>
    [JsonProperty("prompt_eval_duration")]
    public long PromptEvalDuration { get; set; }

    /// <summary>
    /// Anzahl der Eval-Schritte während der Generierung.
    /// </summary>
    [JsonProperty("eval_count")]
    public int EvalCount { get; set; }

    /// <summary>
    /// Dauer der Evaluation in Millisekunden.
    /// </summary>
    [JsonProperty("eval_duration")]
    public long EvalDuration { get; set; }

    /// <summary>
    /// Eine Liste von Antwortmöglichkeiten, die vom LLM generiert wurden.
    /// </summary>
    [JsonProperty("choices")]
    public OllamaChoice[] choices { get; set; }

    /// <summary>
    /// Wandelt einen JSON-String in ein OllamaResponse-Objekt um.
    /// </summary>
    /// <param name="json">Der JSON-String, der geparst werden soll.</param>
    /// <returns>Ein OllamaResponse-Objekt, falls das Parsing erfolgreich war.</returns>
    public static OllamaResponse FromJson(string json)
    {
        return JsonConvert.DeserializeObject<OllamaResponse>(json);
    }
}