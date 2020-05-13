using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine;
using UnityScript.Steps;

public class GameManagerPandonomics : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text week_Text; //Text in the sidebar
    public TMP_Text stats_Text; //Text in the sidebar
    public TMP_Text weekStart_Text; //Window that shows at beginning of week
    public TMP_Text weekEnd_Header_Text; //Window that is shown at the end of the week along with financial stats
    public TMP_Text weekEnd_Earnings_Text; 
    public GameObject endOfCycle_Panel; //Panel that pops up when the week is over

    [Header("Settings")]
    [SerializeField] private float cycleDuration; //Length of a day/week in seconds;
    [SerializeField] private int maxTicks; //ticks per week
    [SerializeField] private int gameTime; //complete duration of the game in weeks
    [SerializeField] private int startingSum; //Amount of money you have at the start of the game

    [Header("Values")]
    public float[] startingRandomValues; //random values for the stockgroups
    public AktienGruppe[] allStockGroups; //all of the groups
    public TextMeshProUGUI[] textStockGroups; //testtexte
    public float[,] allValues; //first Value is the Stock Group, Second the current week;
    public float[] beginningOfDayValues; //wert am anfang des tages
    public float[] dailyTickChange; //calculated change per tick
    public float[] currentValues; //all current values

    //----------------------------------------------------------------------------------------//

    //Stats
    private int numberOfGroups; //how much different groups are in the game
    private int currentTick; //currenttick of the gametimeunit

    //Other
    [HideInInspector] public float tickDuration; //Amount of time between ticks;    
    [HideInInspector] public float remainingTime; //Time left before end of cycle
    private bool cycleOn; //Are we currently in a cycle?
    private float cycleStartTime; //For timekeeping

    void Start()
    {
        tickDuration = cycleDuration / maxTicks;
        Message.AddListener<GameEventMessage>(OnMessage);

        InitializeStocks();

        StartGame();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message.EventName == "StartCycle")
        {
            StartCoroutine(Cycle());
        }
    }

    void UpdateUI()
    {
        int remaining = (int)((cycleStartTime + cycleDuration) - Time.time);
        week_Text.text = "Week " + (PlayerPrefs.GetInt("CurrentWeek") + 1) + "\nRemaining Time: " + remaining + "\nTick Duration: " + tickDuration;
        stats_Text.text = "Bank Account: " + PlayerPrefs.GetFloat("CurrentAmount") + "€\nWeekly Earnings: " + PlayerPrefs.GetFloat("WeeklyEarnings") + "€";
    }

    private IEnumerator Cycle()
    {
        cycleStartTime = Time.time;
        StartWeek();

        while (cycleOn)
        {
            Tick();

            yield return new WaitForSeconds(tickDuration);
        }

        endOfCycle_Panel.SetActive(true);
    }

    private void InitializeStocks()
    {
        numberOfGroups = allStockGroups.Length;
        startingRandomValues = new float[numberOfGroups];
        allValues = new float[numberOfGroups, gameTime + 1];
        dailyTickChange = new float[numberOfGroups];
        beginningOfDayValues = new float[numberOfGroups];
        currentValues = new float[numberOfGroups];

        for (int i = 0; i < numberOfGroups; i++)
        {
            allStockGroups[i].calculateGrowth();
            startingRandomValues[i] = Random.Range(47.8f, 512.6f);
            allValues[i, 0] = startingRandomValues[i];
        }
        for (int i = 0; i < gameTime; i++)
        {
            for (int j = 0; j < numberOfGroups; j++)
            {
                allValues[j, i + 1] = allValues[j, i] * (1 + allStockGroups[j].allGrowthValues[i]);
            }
        }
    }

    void StartGame()
    {
        //Reset Values
        PlayerPrefs.SetFloat("CurrentAmount", startingSum); 
        PlayerPrefs.SetInt("CurrentWeek", 0); 
        PlayerPrefs.SetFloat("WeeklyEarnings", 0); 

        //Set Initial Stock Values
        for (int i = 0; i < textStockGroups.Length; i++)
        {
            textStockGroups[i].text = "" + allValues[i, 0];
        }
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = allValues[i, 0];
        }
    }

    public void Tick()
    {
        currentTick++;
        if (currentTick <= maxTicks)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                //Update Values
                currentValues[i] += dailyTickChange[i];

                //Update UI
                textStockGroups[i].text = "" + currentValues[i];
            }
        }
        else
        {
            EndWeek();
        }
    }

    public void StartWeek()
    {
        cycleOn = true;
        endOfCycle_Panel.SetActive(false);
        currentTick = 0;
        if (PlayerPrefs.GetInt("CurrentWeek") < gameTime)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                beginningOfDayValues[i] = currentValues[i];
                dailyTickChange[i] = (allValues[i, PlayerPrefs.GetInt("CurrentCycle") + 1] - currentValues[i]) / maxTicks;
            }
        }
    }

    void EndWeek()
    {
        cycleOn = false;
        PlayerPrefs.SetInt("CurrentWeek", PlayerPrefs.GetInt("CurrentWeek") + 1);

        //UI
        weekStart_Text.text = "Week " + (PlayerPrefs.GetInt("CurrentWeek") + 1);
        weekEnd_Header_Text.text = "Week " + (PlayerPrefs.GetInt("CurrentWeek"));
        weekEnd_Earnings_Text.text = "You have earned " + PlayerPrefs.GetFloat("WeeklyEarnings") + " this week.\n\nYour total balance is " + PlayerPrefs.GetFloat("CurrentAmount");

        //Update Stats
        PlayerPrefs.SetFloat("CurrentAmount", PlayerPrefs.GetFloat("CurrentAmount") + PlayerPrefs.GetFloat("WeeklyEarnings"));
        PlayerPrefs.SetFloat("WeeklyEarnings", 0);
    }

    void EndGame()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = 0;
            dailyTickChange[i] = 0;
        }
    }
}
