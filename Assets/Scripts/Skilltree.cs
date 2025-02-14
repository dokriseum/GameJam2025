using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Skilltree : MonoBehaviour
{
    public static Skilltree instance;
    public List<Skill_SO> learnedSkills;
    
    public List<Skill> allSkills;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateSkillStates();
    }

    public bool GetIsSkillNotLearnedYet(Skill_SO skill)
    {
        return !learnedSkills.Contains(skill);
    }

    public void AddNewSkill(Skill_SO skill)
    {
        Debug.Log("learned new skill " + skill.name);
        learnedSkills.Add(skill);
    }

    public bool CanAddThisSkill(Skill_SO skill)
    {
        return GetIsSkillNotLearnedYet(skill) 
        && PublicOpinion.instance.GetCurrentOpinion() >= skill.minPopularityScoreToUnlock 
        && PublicOpinion.instance.GetCanByWithOpinion(10);
    }

    public void UpdateSkillStates()
    {
        foreach(Skill skill in allSkills)
        {
            skill.SetInteractableState(CanAddThisSkill(skill.linkedSkill));
        }
    }
}
