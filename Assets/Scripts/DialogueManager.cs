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
    public List<DialogueOption> dialogParameters;

public Transform spawnArea;
    public GameObject dialoguePrefabModerator, dialoguePrefabPolitician, empty;
    public GameObject replyOptions;

    public ScrollRect scrollRect;


    private GameObject currentReplyGO;
    public GameObject[] replyIcons;
    public float waitingTimeAfterModeratorAsked;
    private int currentIndex;

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
        /*
        for(int i = 0; i < 1; i++)
        {
            replyIcons[i].SetActive(true);
            replyIcons[i].GetComponent<SkillReply>().InstantiateSkill(dialogParameters[currentIndex].possibleModifications[i]);
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
        string hierKommtDieAntwortRein = "An Ihre Frage kann ich mich leider nicht erinnern.";

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
