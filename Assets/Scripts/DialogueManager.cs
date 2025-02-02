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
        int possibleModsThisReply = dialogParameters[currentIndex].possibleModifications.Length;
        for(int i = 0; i < 2; i++)
        {
            replyIcons[i].SetActive(true);
            replyIcons[i].GetComponent<SkillReply>().InstantiateSkill(dialogParameters[currentIndex].possibleModifications[i]);
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