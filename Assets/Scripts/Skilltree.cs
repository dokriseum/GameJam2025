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

    public bool GetIsSkillLearned(Skill_SO skill)
    {
            return learnedSkills.Contains(skill);
    }

    public void AddNewSkill(Skill_SO skill)
    {
        Debug.Log("learned new skill " + skill.name);
        learnedSkills.Add(skill);
    }

    public bool CanAddThisSkill(Skill skill)
    {
        if(skill.isBeginSkill) return true;
    Debug.Log("CAN ADD THIS SKILL");
        Skill_SO skillSO = skill.linkedSkill;

        Debug.Log(skill.name + GetIsSkillNotLearnedYet(skillSO) + GetHasUnlockedPreviousSkills(skill) + (PublicOpinion.instance.GetCurrentOpinion() >= skillSO.minPopularityScoreToUnlock) + PublicOpinion.instance.GetCanByWithOpinion(10));
        return GetIsSkillNotLearnedYet(skillSO)
        && GetHasUnlockedPreviousSkills(skill)
        && (PublicOpinion.instance.GetCurrentOpinion() >= skillSO.minPopularityScoreToUnlock) 
        && PublicOpinion.instance.GetCanByWithOpinion(10);
    }

    private bool GetHasUnlockedPreviousSkills(Skill skill)
    {
        Debug.Log("skill checking for hasUnlockedPrevious: " + skill + " amount of needed skills " + skill.neededSkillsUnlocked.Length);
        for(int i = skill.neededSkillsUnlocked.Length - 1; i >= 0; i--)
        {
            Debug.Log(skill.name + " " + i + " AA");
            if(skill.neededSkillsUnlocked[i].isBeginSkill) return true;
            if(skill.neededSkillsUnlocked[i].IsSkillUnlocked()) return true;
        }
        Debug.Log(skill + " found no skills unlocked");
        return false;
    }
    
    public void UpdateSkillStates()
    {
        foreach(Skill skill in allSkills)
        {
            // Setze den Button-Status, ob die Fähigkeit gekauft werden kann.
            skill.SetInteractableState(CanAddThisSkill(skill));
        }
    }
}