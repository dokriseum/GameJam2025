using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEditor.ShortcutManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public List<DialogueOption> Moderationsfragen;

public Transform spawnArea;
    public GameObject dialoguePrefabModerator, dialoguePrefabPolitician;
    public GameObject replyOptions;

    private GameObject currentReplyGO;

    public void InstantiateNewDialogue(Skill_SO modifier, int index)
    {
        currentReplyGO = Instantiate(dialoguePrefabPolitician, spawnArea);
        if(modifier)
        {
            StartCoroutine(KIAntwort(modifier, index));
        }
        else
        {
            SetTextForReply(Moderationsfragen[index].neutraleAntwort);
        }
    }

    public void SetTextForReply(string reply)
    {
        currentReplyGO.GetComponentInChildren<TextMeshProUGUI>().text = reply;
    }

    private IEnumerator KIAntwort(Skill_SO modifier, int index)
    {
        string modifierName = modifier.name;
        string neutraleAntwort = Moderationsfragen[index].neutraleAntwort;
        string hierKommtDieAntwortRein = "An Ihre Frage kann ich mich leider nicht erinnern.";

        // Hier startest du die KI mit dem "Modifier name" (das ist der Skill, z.B. Angst schüren) und lässt damit die neutrale Antwort bearbeiten
         yield return new WaitForSeconds(2f);
          
         // Juchu, antwort ist fertig gebacken
         SetTextForReply(hierKommtDieAntwortRein);
    }
}
