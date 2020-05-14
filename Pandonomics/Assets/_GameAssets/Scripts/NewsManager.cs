using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public TMP_Text newsArticle_Text;

    public Button breakingNews;
    public Button tipOfTheDay;
    public Button moreNews;
    public Button forecast;

    void Start()
    {
        breakingNews.onClick.AddListener(LoadBreakingNews);
        tipOfTheDay.onClick.AddListener(LoadTipOfTheDay);
        moreNews.onClick.AddListener(LoadMoreNews);
        forecast.onClick.AddListener(LoadForecast);
    }

    private void LoadBreakingNews()
    {
        newsArticle_Text.text = "This is the breaking news section";
    }

    private void LoadTipOfTheDay()
    {
        newsArticle_Text.text = "This is the tip of the day section";
    }

    private void LoadMoreNews()
    {
        newsArticle_Text.text = "This is the more news section";
    }

    private void LoadForecast()
    {
        newsArticle_Text.text = "This is the forecast section";
    }
}
