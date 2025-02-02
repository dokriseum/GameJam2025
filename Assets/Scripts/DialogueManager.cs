using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEditor.ShortcutManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{    
    public static DialogueManager instance;
    public List<DialogueOption> dialogParameters;
    public Transform spawnArea;
    public ScrollRect scrollRect;
    public GameObject dialoguePrefabModerator, dialoguePrefabPolitician, empty, replyOptions;
    private GameObject currentReplyGO;
    public GameObject[] replyIcons;
    public float waitingTimeAfterModeratorAsked;
    private int currentIndex;

    private void Start()
    {
        instance = this;
    }

    public void BeginGame()
    {
        currentIndex = 0;
        replyOptions.SetActive(false);
        StartCoroutine(StartNewDialogue());
    }

    public void StartReply(Skill_SO modifier)
    {
        replyOptions.SetActive(false);

        if(modifier)
        {
            StartCoroutine(KIAntwort(modifier, currentIndex));
        }
        else
        {
            SetTextForReply(dialogParameters[currentIndex].neutraleAntwort);
        }

        StartCoroutine(WaitAfterReplyChosen());
    }

    private void GoToNextQuestion()
    {
        currentIndex++;
        StartCoroutine(StartNewDialogue());
    }

    public void StartReplyFromSkill(SkillReply reply)
    {
        StartReply(reply.linkedSkill);
    }

    public void StartReply()
    {
        StartReply(null);
    }

    public void SetTextForReply(string reply)
    {
        currentReplyGO.GetComponentInChildren<TextMeshProUGUI>().text = reply;
    }

    private IEnumerator StartNewDialogue()
    {
        Debug.Log("Starting new dia");
        replyOptions.SetActive(false);
        GameObject modQuestion = Instantiate(dialoguePrefabModerator, spawnArea);
        SetQuestionByModerator(modQuestion.GetComponentInChildren<TextMeshProUGUI>(), dialogParameters[currentIndex].Moderationsfrage);
        Instantiate(empty, spawnArea);
        ScrollToBottom();
        yield return new WaitForSeconds(waitingTimeAfterModeratorAsked);
        Instantiate(empty, spawnArea);
        currentReplyGO = Instantiate(dialoguePrefabPolitician, spawnArea);
        ScrollToBottom();
        replyOptions.SetActive(true);
        UpdateReplyOptions();
    }

    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0 ;
    }

    public void UpdateReplyOptions()
    {

        TextMeshProUGUI neutralText = replyOptions.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        neutralText.text = dialogParameters[currentIndex].neutraleAntwort;

        int possibleModsThisReply = dialogParameters[currentIndex].possibleModifications.Length;
        
        // Hole alle möglichen Modifikationen.
        {
            Skill_SO[] possibleMods;
            possibleMods = dialogParameters[currentIndex].possibleModifications;
        }

        // Filtere anhand des Skilltrees: nur Fähigkeiten anzeigen, die noch nicht gelernt wurden und freigeschaltet werden können.
        List<Skill_SO> availableMods = new List<Skill_SO>();
        foreach(Skill_SO mod in possibleModsThisReply)
        {
            if(Skilltree.instance.CanAddThisSkill(mod))
            {
                availableMods.Add(mod);
            }
        }
    
        // Nun: Ist die Anzahl der verfügbaren Fähigkeiten >= 3?
        if (availableMods.Count >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                replyIcons[i].SetActive(true);
                replyIcons[i].GetComponent<SkillReply>().InstantiateSkill(availableMods[i]);
            }
        }
        else
        {
            // Hier kannst du entscheiden, ob du einen Fallback-Dialog starten oder nur so viele Optionen anzeigen möchtest.
            Debug.Log("Nicht genügend passende Fähigkeiten freigeschaltet.");
            // Beispiel: Zeige alle verfügbaren Fähigkeiten an.
            for (int i = 0; i < availableMods.Count; i++)
            {
                replyIcons[i].SetActive(true);
                replyIcons[i].GetComponent<SkillReply>().InstantiateSkill(availableMods[i]);
            }
            // Deaktiviere restliche Icons
            for (int i = availableMods.Count; i < replyIcons.Length; i++)
            {
                replyIcons[i].SetActive(false);
            }
        }
    }

    public void SetQuestionByModerator(TextMeshProUGUI toTextObj, string withReply)
    {
        toTextObj.text = withReply;
    }

    private IEnumerator KIAntwort(Skill_SO modifier, int index)
    {
        string modifierName = modifier.name;
        string neutraleAntwort = dialogParameters[index].neutraleAntwort;
        string hierKommtDieAntwortRein = "An Ihre Frage kann ich mich leider nicht erinnern.";
        LLMRunner.instance.StartRequest(modifierName, neutraleAntwort);
        // Hier startest du die KI mit dem "Modifier name" (das ist der Skill, z.B. Angst schüren) und lässt damit die neutrale Antwort bearbeiten
        //yield return new WaitForSeconds(2f);
        
        // Version 1 mit OllamaResponse-Onjekt
        OllamaResponse kiAntwortOR = null;
        yield return LLMRunner.instance.WaitForResponse((response) =>
        {
            kiAntwortOR = response;
        });
        if (kiAntwortOR != null && kiAntwortOR.choices != null && kiAntwortOR.choices.Length > 0)
        {
            SetTextForReply(kiAntwortOR.Response); // Nur den generierten Text setzen
        }
        else
        {
            SetTextForReply("Fehler beim Laden der Antwort.");
        }
        
        // Version 2 mit String
        /****
        yield return LLMRunner.instance.WaitForResponse((responseText) =>
        {
            hierKommtDieAntwortRein = responseText;
        });
        yield return LLMRunner.instance.WaitForResponse();
         *****/
    }

    private IEnumerator WaitAfterReplyChosen()
    {
        yield return new WaitForSeconds(waitingTimeAfterModeratorAsked);
        Debug.Log("Done waiting");
        GoToNextQuestion();
    }
}