using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Skill_SO linkedSkill;

    private void Start()
    {

    }

    public void BuySkill()
    {
        Skilltree.instance.AddNewSkill(linkedSkill);
    }

    public void SetInteractableState(bool state)
    {

    }
}
