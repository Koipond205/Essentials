using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinUIManager : MonoBehaviour
{
    int progressAmount;
    public TextMeshProUGUI progressMeter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressMeter.text = "x0";
        Coins.OnCollect += IncreaseProgressAmount;
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressMeter.text = "x" + progressAmount;
        if(progressAmount >= 100)
        {
            //Level complete!
            Debug.Log("Level Complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
