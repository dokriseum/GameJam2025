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

public class LLMRunner : MonoBehaviour
{
    public static LLMRunner instance;
    public TMP_Text responseTextBox;  // TextMeshPro für UI-Ausgabe

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
                    OllamaResponse response = JsonConvert.DeserializeObject<OllamaResponse>(responseJson);
                    if (response != null && response.choices != null && response.choices.Length > 0)
                    {
                     Debug.Log("RESPONSE: " + response.choices[0].text);
                        responseTextBox.text = response.choices[0].text;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"JSON Parsing Error: {ex.Message}\nResponse: {responseJson}");
                }
            }
        }
    }
}
