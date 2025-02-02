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

    public TextMeshProUGUI text;

    private void Start()
    {
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

        Debug.Log(gameObject.name + " setting up. II: " + iconImg);
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
    }

    public void SetInteractableState(bool state)
    {
        button.interactable = state;
    }
}
