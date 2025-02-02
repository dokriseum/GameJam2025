using UnityEngine;
using TMPro;

public class ScoreUpdated : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void InstantiateScoreAdd(int score)
    {
        text.text = "+" + score.ToString();
        Destroy(gameObject, 3f);
    }
}
