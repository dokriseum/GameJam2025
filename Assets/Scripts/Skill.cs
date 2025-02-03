using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Skill : MonoBehaviour
{
    public Skill_SO linkedSkill;
    public Image iconImg;
    public Button button;
    public Color colorWhenBought;
    public Skill[] neededSkillsUnlocked;
    public Image[] linkedLinesIncoming;

    public TextMeshProUGUI text;

    public bool isBeginSkill;

    private void Start()
    {
        if(isBeginSkill) return;
        if (linkedSkill == null)
        {
            Debug.LogWarning(gameObject.name + ": linkedSkill is not set!");
            return;
        }
        if (iconImg == null)
        {
            Debug.LogWarning(gameObject.name + ": iconImg is not set!");
            return;
        }
        if (text == null)
        {
            Debug.LogWarning(gameObject.name + ": text is not set!");
            return;
        }
        iconImg.sprite = linkedSkill.icon;
        text.text = linkedSkill.name;
        if(linkedSkill.icon && iconImg)
        {
            iconImg.sprite = linkedSkill.icon;
        }
    }

    private void Update()
    {
        
    }

    public void BuySkill()
    {
        Skilltree.instance.AddNewSkill(linkedSkill);
        ColorBlock buttonColors = button.colors;
        buttonColors.disabledColor = colorWhenBought;
        button.colors = buttonColors;
    }

    public void SetInteractableState(bool state)
    {
        button.interactable = state;
    }

    public bool IsSkillUnlocked()
    { 
        Debug.Log(name + " is unlocked: " + Skilltree.instance.GetIsSkillLearned(linkedSkill));
        return Skilltree.instance.GetIsSkillLearned(linkedSkill);
    }
}