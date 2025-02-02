using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PublicOpinion : MonoBehaviour
{
    public static PublicOpinion instance;
    private int currentOpinion;
    private int opinionToSpend;
    public TextMeshProUGUI opinionText;
    public Image opinionBar;

    public void Awake()
   {
      instance = this;
      currentOpinion = opinionToSpend = 100;
   }

    public void IncreaseOpinion(int byScore)
    {
        currentOpinion += byScore;
        if(currentOpinion >= 100)
        {
            GameWon();
        }
    }

    public void BuyWithOpinion(int opinionValue)
    {
        opinionToSpend -= opinionToSpend;
    }

    public int GetCurrentOpinion()
    {
        return currentOpinion;
    }

    public bool GetCanByWithOpinion(int opinionValue)
    {
        return opinionToSpend >= opinionValue;
    }

    private void GameWon()
    {

    }
}