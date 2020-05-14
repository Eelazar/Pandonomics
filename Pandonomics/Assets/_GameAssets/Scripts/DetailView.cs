using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : MonoBehaviour
{
    [Header("References")]
    public GameManagerPandonomics gameManager;
    public Stock defaultStock;

    [Header("UI")]
    [SerializeField] private Image stockIcon;
    [SerializeField] private TMP_Text stockAmount;
    [SerializeField] private TMP_Text stockWorth;
    [SerializeField] private Button buy_Btn;
    [SerializeField] private Button sell_Btn;
    [SerializeField] private Button multiplier_Btn;

    private Stock currentStock;

    private TMP_Text multiplier_Text;
    private int currentMultiplier = 1;

    void Start()
    {
        ShowStockDetail(defaultStock);

        buy_Btn.onClick.AddListener(BuyStock);
        sell_Btn.onClick.AddListener(SellStock);
        multiplier_Btn.onClick.AddListener(ChangeMultiplier);

        multiplier_Text = multiplier_Btn.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
         
    }

    public void ShowStockDetail(Stock stock)
    {
        currentStock = stock;

        stockIcon.sprite = currentStock.icon;
        stockAmount.text = "Owned: " + PlayerPrefs.GetInt("AmountOwned_" + currentStock.id);
        stockWorth.text = "Worth: " + (PlayerPrefs.GetInt("AmountOwned_" + currentStock.id) * gameManager.currentValues[currentStock.id - 1]) + "€";
    }
    private void UpdateUI()
    {
        stockAmount.text = "Owned: " + PlayerPrefs.GetInt("AmountOwned_" + currentStock.id);
        stockWorth.text = "Worth: " + (PlayerPrefs.GetInt("AmountOwned_" + currentStock.id) * gameManager.currentValues[currentStock.id - 1]) + "€";
    }


    //Listeners
    private void BuyStock()
    {
        if (currentStock != null)
        {
            //Get the total price of the stocks
            float price = gameManager.currentValues[currentStock.id - 1] * currentMultiplier;

            //Check if player has enough money
            if (PlayerPrefs.GetFloat("CurrentBalance") >= price)
            {
                //Set the new balance
                PlayerPrefs.SetFloat("CurrentBalance", PlayerPrefs.GetFloat("CurrentBalance") - price);
                //Set the amount of stock owned
                PlayerPrefs.SetInt("AmountOwned_" + currentStock.id, PlayerPrefs.GetInt("AmountOwned_" + currentStock.id) + currentMultiplier);

                PlayerPrefs.SetFloat("WeeklyExpenses", PlayerPrefs.GetFloat("WeeklyExpenses") + price);
                UpdateUI();
            }
            else Debug.Log("Not enough money to pay for: " + price);
        }
        else Debug.Log("currentStock == null");
    }

    private void SellStock()
    {
        if (currentStock != null)
        {
            int amountOwned = PlayerPrefs.GetInt("AmountOwned_" + currentStock.id);

            if (amountOwned > 0)
            {
                float price = 0;

                //Check if player is trying to sell more than they own
                if (currentMultiplier > amountOwned)
                {
                    //Get the total price of the stocks
                    price = gameManager.currentValues[currentStock.id - 1] * amountOwned;

                    //Set the new balance
                    PlayerPrefs.SetFloat("CurrentBalance", PlayerPrefs.GetFloat("CurrentBalance") + price);
                    //Set the amount of stock owned
                    PlayerPrefs.SetInt("AmountOwned_" + currentStock.id, 0);
                }
                else
                {
                    //Get the total price of the stocks
                    price = gameManager.currentValues[currentStock.id - 1] * currentMultiplier;

                    //Set the new balance
                    PlayerPrefs.SetFloat("CurrentBalance", PlayerPrefs.GetFloat("CurrentBalance") + price);
                    //Set the amount of stock owned
                    PlayerPrefs.SetInt("AmountOwned_" + currentStock.id, amountOwned - currentMultiplier);
                }

                PlayerPrefs.SetFloat("WeeklyEarnings", PlayerPrefs.GetFloat("WeeklyEarnings") + price);
                UpdateUI();
            }            
        }
        else Debug.Log("currentStock == null");
    }

    private void ChangeMultiplier()
    {
        switch (currentMultiplier)
        {
            case 1:
                currentMultiplier = 10;
                break;

            case 10:
                currentMultiplier = 100;
                break;

            case 100:
                currentMultiplier = 1000;
                break;

            case 1000:
                currentMultiplier = 1;
                break;

            default:
                currentMultiplier = 1;
                break;
        }

        multiplier_Text.text = "x" + currentMultiplier;
    }
}
