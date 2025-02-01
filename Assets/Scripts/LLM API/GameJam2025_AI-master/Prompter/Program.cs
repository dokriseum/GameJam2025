using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp;

class Program
{
    private static string cName, cFrage, cStil, cInfos, cModel;
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        
        // Promptvariablen füttern
        cFrage = args[0].Replace("\"", "");
        cStil = args[1].Replace("\"", "");
        cInfos = args[2].Replace("\"", "");
        cModel = args[3];
        var prompt = Prompts.PROMPT_001(cFrage, cStil, cInfos);
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
        
        // Füge den Accept-Header hinzu
        request.Headers.Add("Accept", "application/json");

        // Erstelle den JSON-Inhalt und setze die Content-Type-Header über das StringContent-Objekt
        //request.Content = new StringContent(prompt, Encoding.UTF8, "application/json");
        var content = new StringContent(
            JsonConvert.SerializeObject(new
            {
                model = cModel,//"deepseek-r1:7b",
                prompt = prompt,
                stream = false
            }), 
            Encoding.UTF8, 
            "application/json"
        );
        
        // Setze den Inhalt der Anfrage
        request.Content = content;

        try
        {
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(result+"\n\n\n\n\n");
                DeepSeekResponse dsr = DeepSeekResponse.FromJson(result);
                Console.WriteLine("\n\n\n"+prompt+"\n\n\n");
                Console.WriteLine(dsr.Response);
            }
            else
            {
                Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Fertig!");
    }
}