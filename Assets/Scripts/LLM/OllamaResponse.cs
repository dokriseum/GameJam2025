using UnityEngine;
using System;

[System.Serializable]
public class OllamaResponse
{ 
    public string id, @object, model, system_fingerprint; 
    public long created;
    public OllamaChoice[] choices; 
    public OllamaUsage usage;
}