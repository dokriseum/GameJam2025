using UnityEngine;

// Sobald Frage gestellt wird, plobt das hier auf.
// Und diese bis zu 3 Skill Icons sollen mit den möglichen Modifikationen mit den Icons übernehmen. Und dafür instanziiert werden.

public class SkillReply : Skill
{
    private string replyPreview; // 1 satz als preview
    public void ThisSkillWasChosen()
    {
        // Füge die ausgewählte Fähigkeit zum Skilltree hinzu.
        if (linkedSkill != null && Skilltree.instance.CanAddThisSkill(linkedSkill))
        {
            Skilltree.instance.AddNewSkill(linkedSkill);
            // Optional: Rückmeldung an den Spieler, Animationen etc.
        }
        else
        {
            Debug.Log("Diese Fähigkeit kann nicht ausgewählt werden.");
        }
    }


    public void InstantiateSkill(Skill_SO withSkill)
    {
        linkedSkill = withSkill;
        iconImg.sprite = withSkill.icon;
        text.text = withSkill.name;
    }
}