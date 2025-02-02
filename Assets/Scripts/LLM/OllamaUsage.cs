[System.Serializable]
public class OllamaUsage
{ 
    /// <summary>
    /// Anzahl der Tokens, die im Prompt verwendet wurden.
    /// </summary>
    public int prompt_tokens;
    
    /// <summary>
    /// Anzahl der Tokens, die in der generierten Antwort verwendet wurden.
    /// </summary>
    public int completion_tokens;
    
    /// <summary>
    /// Gesamtsumme der verwendeten Tokens.
    /// </summary>
    public int total_tokens;
}