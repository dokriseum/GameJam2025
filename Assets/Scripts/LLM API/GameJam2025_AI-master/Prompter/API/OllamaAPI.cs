using System.Diagnostics;

namespace ConsoleApp;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class OllamaAPI
{
    private HttpClient client = new HttpClient();

    public async void Start()
    {
        var response = await GetCompletion("Gib mir einen lustigen Witz.");
        //Debug.Log(response);
    }

    public async Task<string> GetCompletion(string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:11434/api/generate");
        request.Headers.Add("Accept", "application/json");
        request.Content = new StringContent(JsonConvert.SerializeObject(new
        {
            model = "deepseek-r1",
            prompt = prompt,
            stream = false
        }));

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}