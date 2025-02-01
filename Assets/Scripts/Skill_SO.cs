using UnityEngine;
using System.Collections;



[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill_SO")]
public class Skill_SO : ScriptableObject
{
    [Range(0,100)]public int popularityScoreToUnlock;
public string titel;
public string beschreibung;
public string prompt;
    
}
