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
public GameObject skillsUIButton;

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

        PublicOpinion.instance.IncreaseOpinion(6);

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
        skillsUIButton.SetActive(true);
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
        
        // Filtere anhand des Skilltrees: nur Fähigkeiten anzeigen, die noch nicht gelernt wurden und freigeschaltet werden können.
        List<Skill_SO> availableMods = new List<Skill_SO>();

        // Hole alle möglichen Modifikationen.
        Skill_SO[] possibleMods = dialogParameters[currentIndex].possibleModifications;

        // Alle Icons aktivieren, damit sie sichtbar bleiben
        for (int i = 0; i < replyIcons.Length; i++)

        {
            replyIcons[i].SetActive(i < possibleMods.Length);
        }

        // Gehe durch alle möglichen Mods und setze ihre Anzeige
        for (int i = 0; i < possibleMods.Length; i++)
        {
            Skill_SO mod = possibleMods[i];
            SkillReply skillReply = replyIcons[i].GetComponent<SkillReply>();

            if (Skilltree.instance.GetIsSkillLearned(mod))
            {
                skillReply.InstantiateSkill(mod); // Zeigt den Skill normal an
                skillReply.SetStateOfLockedUI(false); // Falls du ein visuelles Lock hast
            }
            else
            {
                skillReply.InstantiateSkill(mod); // Zeigt den Skill trotzdem an
                skillReply.SetStateOfLockedUI(true);  // Zeigt, dass er gesperrt ist (z.B. grau hinterlegt)
            }
        }

        
        /*

        foreach (Skill_SO mod in possibleMods)
        {
            if (Skilltree.instance.GetIsSkillLearned(mod))
            {
                availableMods.Add(mod);
            }
        }

        // Aktiviere so viele Icons, wie es verfügbare Modifikationen gibt (maximal 3)
        int modsToShow = Mathf.Min(possibleModsThisReply, availableMods.Count);

        for (int i = 0; i < modsToShow; i++)
        {
            replyIcons[i].SetActive(true);
            replyIcons[i].GetComponent<SkillReply>().InstantiateSkill(availableMods[i]);
        }

        // Deaktiviere die restlichen Icons, falls es weniger verfügbare Modifikationen gibt
        for (int i = modsToShow; i < replyIcons.Length; i++)
        {
            replyIcons[i].SetActive(false);
        }
        */
    }

    public void SetQuestionByModerator(TextMeshProUGUI toTextObj, string withReply)
    {
        toTextObj.text = withReply;
    }

    private IEnumerator KIAntwort(Skill_SO modifier, int index)
    {
        string modifierName = modifier.name;
        string neutraleAntwort = dialogParameters[index].neutraleAntwort;
        string hierKommtDieAntwortRein = "An Ihre Frage kann ich mich leider nicht erinnern. (Fehler mit der KI-Generierung)";

        // Hier startest du die KI mit dem "Modifier name" (das ist der Skill, z.B. Angst schüren) und lässt damit die neutrale Antwort bearbeiten
         yield return new WaitForSeconds(2f);
          
         // Juchu, antwort ist fertig gebacken
         SetTextForReply(hierKommtDieAntwortRein);         
    }

    private IEnumerator WaitAfterReplyChosen()
    {
        yield return new WaitForSeconds(waitingTimeAfterModeratorAsked);
        Debug.Log("Done waiting");
        GoToNextQuestion();

    }
}