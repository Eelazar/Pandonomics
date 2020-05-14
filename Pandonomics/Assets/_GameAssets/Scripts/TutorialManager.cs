using Doozy.Engine;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public UIPopup news_Tutorial;
    public UIPopup stock_Tutorial;

    void Start()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    void Update()
    {
        
    }

    private void OnMessage(GameEventMessage message)
    {
        switch (message.EventName)
        {
            case "NewsTutorial":
                if(PlayerPrefs.GetInt("NewsTutorial") == 0)
                {
                    news_Tutorial.Show();
                    PlayerPrefs.SetInt("NewsTutorial", 1);
                }                
                break;

            case "StockTutorial":
                if (PlayerPrefs.GetInt("StockTutorial") == 0)
                {
                    stock_Tutorial.Show();
                    PlayerPrefs.SetInt("StockTutorial", 1);
                }
                break;

            default:
                break;
        }
    }
}
