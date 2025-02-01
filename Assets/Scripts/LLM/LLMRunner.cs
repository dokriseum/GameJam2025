using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LLMRunner : MonoBehaviour
{
    public static LLMRunner instance;
    public TMP_Text responseTextBox;
    private OllamaResponse latestResponse; // Hier wird die gesamte Antwort gespeichert

    private void Awake()
    {
        instance = this;
    }

    public void StartRequest(string model, string prompt)
    {
        StartCoroutine(FetchResponse(model, prompt));
    }

    private IEnumerator FetchResponse(string model, string prompt)
    {
        var requestPayload = new
        {
            model = model,
            prompt = prompt,
            stream = false
        };

        string sendJSON = JsonConvert.SerializeObject(requestPayload);
        Debug.Log("Sending request: " + sendJSON);

        using (UnityWebRequest webRequest = new UnityWebRequest("http://192.168.0.180:11434/api/generate", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(sendJSON);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.useHttpContinue = false;
            webRequest.SetRequestHeader("Upgrade-Insecure-Requests", "1");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + webRequest.error);
            }
            else
            {
                string responseJson = webRequest.downloadHandler.text;
                Debug.Log("Server Response: " + responseJson);

                try
                {
                    latestResponse = JsonConvert.DeserializeObject<OllamaResponse>(responseJson);

                    if (latestResponse != null && latestResponse.choices != null && latestResponse.choices.Length > 0)
                    {
                        responseTextBox.text = latestResponse.choices[0].text; // Antwort anzeigen
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"JSON Parsing Error: {ex.Message}\nResponse: {responseJson}");
                }
            }
        }
    }

    public OllamaResponse GetLatestResponse()
    {
        return latestResponse;
    }
    
    /**
     * Beispiel
     * string generatedText = LLMRunner.instance.GetGeneratedText();
     * Debug.Log("Direkte Antwort: " + generatedText);
     */
    
    public string GetGeneratedText()
    {
        return latestResponse != null ? latestResponse.Response : "";
    }
    
    public void ExampleUsage()
    {
        if (LLMRunner.instance != null)
        {
            OllamaResponse response = LLMRunner.instance.GetLatestResponse();
            if (response != null)
            {
                Debug.Log("Model: " + response.Model);
                Debug.Log("Antwort: " + response.choices[0].text);
                Debug.Log("Erstellungsdatum: " + response.CreatedAt);
                Debug.Log("Antwort beendet?: " + response.Done);
                Debug.Log("Gesamtdauer: " + response.TotalDuration);
            }
        }
    }
    
    public void UseGeneratedResponse()
    {
        if (LLMRunner.instance != null)
        {
            OllamaResponse response = LLMRunner.instance.GetLatestResponse();
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                string generatedText = response.Response; // Nur die Antwort extrahieren
                Debug.Log("Generierte Antwort: " + generatedText);

                // Hier kannst du die Antwort weiterverwenden, z.B.:
                MyCustomFunction(generatedText);
            }
        }
    }

    // Beispiel: Verwenden in einer anderen Funktion
    public void MyCustomFunction(string text)
    {
        Debug.Log("Antwort wird weiterverwendet: " + text);
    }

}