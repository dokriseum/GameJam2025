using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LLMRunner : MonoBehaviour
{
   public static LLMRunner instance;
   public Text reponseTextBox;

   public string prompt;

   public void Start()
   {
      instance = this;
      Debug.Log("Starting a request now with prompt: " + prompt);
      StartCoroutine(Send(prompt));
   }
   public IEnumerator Send(string text)
   {
      OllamaRequest request = new OllamaRequest();
      request.model = "mistral:latest";
      request.prompt = text;
      
      string sendJSON = JsonUtility.ToJson(request);
      Debug.Log(sendJSON);
      using (UnityWebRequest webRequest = new UnityWebRequest("http://192.168.0.180:11434/api/generate", "POST"))
      {
         byte[] bodyRaw = Encoding.UTF8.GetBytes(sendJSON);
         webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
         webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         webRequest.SetRequestHeader("Content-Type", "application/json");
         yield return webRequest.SendWebRequest();
         if (webRequest.result !=  UnityWebRequest.Result.Success)
         {
            Debug.LogError(webRequest.error);
         }
         else
         {
            Debug.Log(webRequest.downloadHandler.text);
            string responseJson = webRequest.downloadHandler.text;
            OllamaResponse response = JsonUtility.FromJson<OllamaResponse>(responseJson);
            reponseTextBox.text = response.choices[0].text;
         }
      }
   }
}

