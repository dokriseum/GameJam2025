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
    public Skill[] neededSkillsUnlocked;

    public TextMeshProUGUI text;

    private void Start()
    {
        Debug.Log(gameObject.name + " setting up. II: " + iconImg);
        iconImg.sprite = linkedSkill.icon;
        text.text = linkedSkill.name;
        if(linkedSkill.icon && iconImg)
                iconImg.sprite = linkedSkill.icon;
    }

    private void Update()
    {
        
    }

    public void BuySkill()
    {
        Skilltree.instance.AddNewSkill(linkedSkill);
    }

    public void SetInteractableState(bool state)
    {
        button.interactable = state;
    }
}
