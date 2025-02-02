using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEditor.ShortcutManagement;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public List<DialogueOption> dialogParameters;
    public Transform spawnArea;
    public GameObject dialoguePrefabModerator, dialoguePrefabPolitician, empty, replyOptions;
    private GameObject currentReplyGO;
    public GameObject[] replyIcons;
    public float waitingTimeAfterModeratorAsked;
    private int currentIndex;

    public void BeginGame()
    {
        StartCoroutine(StartNewDialogue());
    }

    public void InstantiateNewDialogue(Skill_SO modifier)
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

        currentIndex++;
    }

    public void SetTextForReply(string reply)
    {
        currentReplyGO.GetComponentInChildren<TextMeshProUGUI>().text = reply;
    }

    private IEnumerator StartNewDialogue()
    {
        replyOptions.SetActive(false);
        Instantiate(dialoguePrefabModerator, spawnArea);
        Instantiate(empty, spawnArea);
        yield return new WaitForSeconds(waitingTimeAfterModeratorAsked);
        Instantiate(empty, spawnArea);
        currentReplyGO = Instantiate(dialoguePrefabPolitician, spawnArea);
        replyOptions.SetActive(true);
        UpdateReplyOptions();
    }

    public void UpdateReplyOptions()
    {
        // Hole alle möglichen Modifikationen.
        Skill_SO[] possibleMods = dialogParameters[currentIndex].possibleModifications;
        // Filtere anhand des Skilltrees: nur Fähigkeiten anzeigen, die noch nicht gelernt wurden und freigeschaltet werden können.
        List<Skill_SO> availableMods = new List<Skill_SO>();
        foreach(Skill_SO mod in possibleMods)
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

    private IEnumerator KIAntwort(Skill_SO modifier, int index)
    {
        string modifierName = modifier.name;
        string neutraleAntwort = dialogParameters[index].neutraleAntwort;
        string hierKommtDieAntwortRein = "An Ihre Frage kann ich mich leider nicht erinnern.";

        // Hier startest du die KI mit dem "Modifier name" (das ist der Skill, z.B. Angst schüren) und lässt damit die neutrale Antwort bearbeiten
         yield return new WaitForSeconds(2f);
          
         // Juchu, antwort ist fertig gebacken
         SetTextForReply(hierKommtDieAntwortRein);
    }
}