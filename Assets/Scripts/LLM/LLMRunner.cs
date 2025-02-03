using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Steuert die Kommunikation mit dem LLM (Large Language Model) und verarbeitet die generierten Antworten.
/// </summary>
public class LLMRunner : MonoBehaviour
{
    /// <summary>
    /// Singleton-Instanz, um global auf den LLMRunner zugreifen zu können.
    /// </summary>
    public static LLMRunner instance;

    /// <summary>
    /// UI-Textfeld, in dem die Antwort angezeigt wird.
    /// </summary>
    public TMP_Text responseTextBox;

    /// <summary>
    /// Speichert die zuletzt empfangene Antwort des LLM.
    /// </summary>
    private OllamaResponse latestResponse;

    /// <summary>
    /// Initialisiert die Singleton-Instanz.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Startet eine Anfrage an das LLM mit dem angegebenen Modell und Prompt.
    /// </summary>
    /// <param name="model">Der Name des Modells, das verwendet werden soll.</param>
    /// <param name="prompt">Der Prompt, der an das Modell gesendet wird.</param>
    public void StartRequest(string model, string prompt)
    {
        StartCoroutine(FetchResponse(model, prompt));
    }

    /// <summary>
    /// Coroutine, die den LLM-Server kontaktiert, um eine Antwort zu generieren.
    /// </summary>
    /// <param name="model">Das zu verwendende Modell.</param>
    /// <param name="prompt">Der an das Modell zu sendende Prompt.</param>
    /// <returns>IEnumerator für die Coroutine.</returns>
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
                        // Zeigt die generierte Antwort im UI-Textfeld an.
                        responseTextBox.text = latestResponse.choices[0].text;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"JSON Parsing Error: {ex.Message}\nResponse: {responseJson}");
                }
            }
        }
    }

    /// <summary>
    /// Gibt die zuletzt empfangene LLM-Antwort zurück.
    /// </summary>
    /// <returns>Die zuletzt empfangene OllamaResponse oder null, falls keine Antwort vorliegt.</returns>
    public OllamaResponse GetLatestResponse()
    {
        return latestResponse;
    }

    /// <summary>
    /// Coroutine, die auf eine vollständige Antwort wartet und diese dann im UI-Textfeld anzeigt.
    /// </summary>
    /// <returns>IEnumerator für die Coroutine.</returns>
    public IEnumerator WaitForResponse()
    {
        while (latestResponse == null || latestResponse.choices == null || latestResponse.choices.Length == 0)
        {
            yield return null;
        }
        responseTextBox.text = latestResponse.choices[0].text;
    }
    
    /// <summary>
    /// Coroutine, die auf eine vollständige Antwort wartet und einen Callback mit der OllamaResponse aufruft.
    /// </summary>
    /// <param name="callback">Action, die mit der vollständigen OllamaResponse aufgerufen wird.</param>
    /// <returns>IEnumerator für die Coroutine.</returns>
    public IEnumerator WaitForResponse(Action<OllamaResponse> callback)
    {
        while (latestResponse == null || latestResponse.choices == null || latestResponse.choices.Length == 0)
        {
            yield return null;
        }
        callback?.Invoke(latestResponse);
    }
    
    /// <summary>
    /// Coroutine, die auf eine vollständige Antwort wartet und einen Callback mit dem generierten Text aufruft.
    /// </summary>
    /// <param name="callback">Action, die mit dem generierten Text aufgerufen wird.</param>
    /// <returns>IEnumerator für die Coroutine.</returns>
    public IEnumerator WaitForResponse(Action<string> callback)
    {
        while (latestResponse == null || latestResponse.choices == null || latestResponse.choices.Length == 0)
        {
            yield return null;
        }
        callback?.Invoke(latestResponse.choices[0].text);
    }
    
    /// <summary>
    /// Extrahiert den generierten Text aus der zuletzt empfangenen Antwort.
    /// </summary>
    /// <returns>Der generierte Text oder ein leerer String, falls keine Antwort vorliegt.</returns>
    public string GetGeneratedText()
    {
        return latestResponse != null ? latestResponse.Response : "";
    }
    
    /// <summary>
    /// Beispielmethode zur Demonstration, wie auf die generierte Antwort zugegriffen werden kann.
    /// </summary>
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
    
    /// <summary>
    /// Verwendet die generierte Antwort, sofern vorhanden, und übergibt sie an eine benutzerdefinierte Funktion.
    /// </summary>
    public void UseGeneratedResponse()
    {
        if (LLMRunner.instance != null)
        {
            OllamaResponse response = LLMRunner.instance.GetLatestResponse();
            if (response != null && !string.IsNullOrEmpty(response.Response))
            {
                string generatedText = response.Response; // Extrahiert nur den generierten Text.
                Debug.Log("Generierte Antwort: " + generatedText);

                // Weiterverarbeitung der Antwort.
                MyCustomFunction(generatedText);
            }
        }
    }

    /// <summary>
    /// Beispielmethode zur Weiterverarbeitung des generierten Texts.
    /// </summary>
    /// <param name="text">Der generierte Text, der weiterverarbeitet werden soll.</param>
    public void MyCustomFunction(string text)
    {
        Debug.Log("Antwort wird weiterverwendet: " + text);
    }
}
