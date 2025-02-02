using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class PopulismResponseGenerator : MonoBehaviour
{
    /// <summary>
    /// Diese Methode nimmt die Fähigkeit, den Basistext und die Moderatorfrage entgegen,
    /// baut daraus einen Prompt und startet den Anfrageprozess.
    /// </summary>
    /// <param name="cFaehigkeit">Die gewählte Fähigkeit (z.B. "Lügen")</param>
    /// <param name="cNeutralAnwort">Der vordefinierte Text, der manipuliert werden soll</param>
    /// <param name="cFrage">Die Frage des Moderators</param>
    public void GenerateResponse(string cFaehigkeit, string cNeutralAnwort, string cFrage, [CanBeNull] string cModel = null)
    {
        if (cModel == null)
        {
            cModel = "mistral:latest";
        }
        // Erstelle den Prompt, der alle Informationen enthält.
        string prompt = BuildPrompt(cFaehigkeit, cNeutralAnwort, cFrage);
        
        // Starte die Anfrage an das LLM (hier musst du ggf. den Modelnamen anpassen)
        LLMRunner.instance.StartRequest(cModel, prompt);
        
        // Starte einen Coroutine, der auf die Antwort wartet und dann die UI aktualisiert.
        StartCoroutine(WaitForAndShowResponse());
    }
    
    /// <summary>
    /// Baut den Prompt zusammen, der an das LLM geschickt wird.
    /// </summary>
    /// <param name="cFaehigkeit">Die Fähigkeit, die verwendet werden soll.</param>
    /// <param name="cNeutralAnwort">Der Basistext, der modifiziert wird.</param>
    /// <param name="cFrage">Die Frage des Moderators.</param>
    /// <returns>Der zusammengesetzte Prompt als string.</returns>
    private string BuildPrompt(string cFaehigkeit, string cNeutralAnwort, string cFrage)
    {
        // Hier wird der Prompt so formatiert, dass er alle nötigen Informationen enthält.
        // Du kannst diesen Text weiter verfeinern, um das gewünschte Verhalten (z.B. populistische Argumentationsmuster) zu erzwingen.
        return $"Moderator: {cFrage}\n\n" +
               $"Basisantwort: {cNeutralAnwort}\n\n" +
               $"Fähigkeit: {cFaehigkeit}\n\n" +
               $"Bitte generiere eine Antwort, die populistische Rhetorik verwendet und die Fähigkeit \"{cFaehigkeit}\" in die Antwort einfließen lässt.";
    }
    
    /// <summary>
    /// Wartet auf die vollständige Generierung der Antwort und aktualisiert dann das Dialogfeld.
    /// </summary>
    private IEnumerator WaitForAndShowResponse()
    {
        // Nutzt die bereitgestellten WaitForResponse-Methoden aus LLMRunner.
        yield return LLMRunner.instance.WaitForResponse((string generatedText) =>
        {
            Debug.Log("Generierte Antwort: " + generatedText);
            // Hier kannst du das Dialogfeld (z.B. über deinen DialogueManager) aktualisieren.
            // Beispiel: DialogueManager.instance.SetTextForReply(generatedText);
        });
    }
}