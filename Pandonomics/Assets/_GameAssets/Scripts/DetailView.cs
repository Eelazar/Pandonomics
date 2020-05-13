using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : MonoBehaviour
{
    public GameManagerPandonomics gameManager;

    [SerializeField] private Image stockIcon;
    [SerializeField] private TMP_Text stockAmount;
    [SerializeField] private TMP_Text stockWorth;

    void Start()
    {
        
    }

    void Update()
    {
         
    }

    public void ShowStockDetail(Stock stock)
    {
        stockIcon.sprite = stock.icon;
        stockAmount.text = "Owned: " + PlayerPrefs.GetInt("AmountOwned_" + stock.id);
        stockWorth.text = "Gotta do this";
    }
}
