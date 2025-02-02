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

    public GameObject opinionChangedPrefab;
    public Transform whereToSpawnOpinionChange;

    public GameObject audience;

    private void Update()
    {
        opinionText.text = currentOpinion.ToString();
    }

    public void Awake()
   {
      instance = this;
      currentOpinion = opinionToSpend = 50;
   }

    public void IncreaseOpinion(int byScore)
    {
        currentOpinion += byScore;
        opinionToSpend += byScore;
        GameObject newObj = Instantiate(opinionChangedPrefab, whereToSpawnOpinionChange);
        newObj.GetComponent<ScoreUpdated>().InstantiateScoreAdd(byScore);
        LeanTween.moveLocalY(audience, -0.003f, 0.2f).setLoopPingPong(byScore);
        RefreshBar();
        if(currentOpinion >= 100)
        {
            GameWon();
        }
    }

    private void RefreshBar()
    {
        Debug.Log(currentOpinion / 100);
        opinionBar.fillAmount = currentOpinion / 100;
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