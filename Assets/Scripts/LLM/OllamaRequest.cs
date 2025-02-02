using UnityEngine;

/// <summary>
/// Repräsentiert die Anfrage an das LLM, inklusive Modell und Prompt.
/// </summary>
[System.Serializable]
public class OllamaRequest
{
    /// <summary>
    /// Das Modell, das für die Anfrage verwendet werden soll.
    /// </summary>
    public string model;
    
    /// <summary>
    /// Der Prompt, der an das LLM gesendet wird.
    /// </summary>
    public string prompt;
}