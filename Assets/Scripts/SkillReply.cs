using UnityEngine;

public class SkillReply : Skill
{
    private string replyPreview; // 1 satz als preview
    public GameObject locked;
    public void ThisSkillWasChosen()
    {
        DialogueManager dialogueMan = (DialogueManager) GameObject.Find("Game Manager").GetComponent<DialogueManager>();
        dialogueMan.StartReplyFromSkill(this);
    }

    public void InstantiateSkill(Skill_SO withSkill)
    {
        linkedSkill = withSkill;
        iconImg.sprite = withSkill.icon;
        text.text = withSkill.name;

        locked.SetActive(!Skilltree.instance.GetIsSkillLearned(withSkill));
    }

    public void SetStateOfLockedUI(bool state)
    {
        locked.SetActive(state);
    }
}
