using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class Cooldown : MonoBehaviour
{
    public Image cooldown;
    public bool coolingDown;
    public float waitTime = 30.0f;

    public int fillAmount; //
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (coolingDown == true)
        {
            //10 pro 30
            cooldown.fillAmount += 10f /waitTime * Time.deltaTime; //Zeit müsste durch anderes Event ausgelöst werden
        }
    }
}