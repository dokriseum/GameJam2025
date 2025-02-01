using UnityEngine;

public class Dialoguemanager: MonoBehaviour
{
    public Text_Frage;
    public Btn_Aufrufen_1;
    public Btn_Aufrufen_2;
    public Btn_Aufrufen_3;

    private Dialogue currentDialogue;
    private int current line index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDialogue = CreateSampleDialogue();

        Btn_Aufrufen_1.onClick.AddListener()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
