using UnityEngine;

public class SkillReply : Skill
{
    private string replyPreview; // 1 satz als preview
    public void ThisSkillWasChosen()
    {

    }

    public void InstantiateSkill(Skill_SO withSkill)
    {
        linkedSkill = withSkill;
        iconImg.sprite = withSkill.icon;
        text.text = withSkill.name;
    }
}
