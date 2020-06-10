using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine;
using UnityScript.Steps;
using Doozy.Engine.UI;

public class GameManagerPandonomics : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text week_Text; //Text in the sidebar
    public TMP_Text stats_Text; //Text in the sidebar
    public TMP_Text weekStart_Text; //Window that shows at beginning of week
    public TMP_Text weekEnd_Header_Text; //Window that is shown at the end of the week along with financial stats
    public TMP_Text weekEnd_Earnings_Text; 
    public UIPopup endOfWeek_Popup; //Panel that pops up when the week is over
    public TMP_Text[] stockPrice_Text; //Text showing the current stock price
    public TMP_Text[] stockChange_Text; //Text showing the current stock price
    public Color green;
    public Color red;

    [Header("Settings")]
    [SerializeField] private float cycleDuration; //Length of a day/week in seconds;
    [SerializeField] private int maxTicks; //ticks per week
    [SerializeField] private int gameTime; //complete duration of the game in weeks
    [SerializeField] private int startingSum; //Amount of money you have at the start of the game

    [Header("Values")]
    public float[] startingRandomValues; //random values for the stockgroups
    public AktienGruppe[] allStockGroups; //all of the groups
    public float[,] allValues; //first Value is the Stock Group, Second the current week;
    public float[] beginningOfDayValues; //wert am anfang des tages
    public float[] dailyTickChange; //calculated change per tick
    public float[] currentValues; //all current values

    //----------------------------------------------------------------------------------------//

    //Stats
    private int numberOfGroups; //how much different groups are in the game
    private int currentTick; //currenttick of the gametimeunit
    public GameObject[] graphs;
    public Canvas graphCanvas;
    public Image basicImage;
    public int thicknessOfGraph=5;
    public Vector3[] currentGraphPosition;
    private Vector3[] intermediatePoints;
    private float[,] allWeekValues;
    private float[] maxWeekValue;
    public float[] graphScale;
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
        stats_Text.text = "Bank Account: " + PlayerPrefs.GetFloat("CurrentBalance") + "€\nWeekly Earnings: " + PlayerPrefs.GetFloat("WeeklyEarnings") + "€";
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

        endOfWeek_Popup.Show();
    }

    private void InitializeStocks()
    {
        numberOfGroups = allStockGroups.Length;
        startingRandomValues = new float[numberOfGroups];
        allValues = new float[numberOfGroups, gameTime + 1];
        dailyTickChange = new float[numberOfGroups];
        beginningOfDayValues = new float[numberOfGroups];
        currentValues = new float[numberOfGroups];
        graphs = new GameObject[numberOfGroups];
        intermediatePoints = new Vector3[numberOfGroups];
        allWeekValues = new float[numberOfGroups,maxTicks];
        maxWeekValue = new float[numberOfGroups];
        graphScale = new float[numberOfGroups];
        currentGraphPosition = new Vector3[numberOfGroups];

        for (int i = 0; i < numberOfGroups; i++)
        {
            allStockGroups[i].calculateGrowth();
            startingRandomValues[i] = Random.Range(47.8f, 512.6f);
            allValues[i, 0] = startingRandomValues[i];
            graphs[i] = new GameObject("Graph of Group" + (i + 1) + "", typeof(RectTransform));
            graphs[i].transform.SetParent(graphCanvas.transform);
            graphs[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            graphs[i].GetComponent<RectTransform>().localPosition = new Vector3(-675, -34, 0f);
            


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
        PlayerPrefs.SetFloat("CurrentBalance", startingSum); 
        PlayerPrefs.SetInt("CurrentWeek", 0); 
        PlayerPrefs.SetFloat("WeeklyEarnings", 0); 
        PlayerPrefs.SetFloat("WeeklyExpenses", 0); 
        for(int i = 0; i < numberOfGroups; i++)
        {
            PlayerPrefs.SetInt("AmountOwned_" + (i + 1), 0);
        }
        PlayerPrefs.SetInt("NewsTutorial", 0);
        PlayerPrefs.SetInt("StockTutorial", 0);

        //Set Initial Stock Values
        for (int i = 0; i < stockPrice_Text.Length; i++)
        {
            stockPrice_Text[i].text = "" + allValues[i, 0];
        }
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = allValues[i, 0];
        }
        
    }

    public void Tick()
    {
        currentTick++;
        if (currentTick < maxTicks)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                
                
                //Update and round Values
                currentValues[i] = allWeekValues[i, currentTick];
                float roundedTickChange = Mathf.Round(dailyTickChange[i] * 100f) / 100f;
                float roundedCurrentValue = Mathf.Round(currentValues[i] * 100f) / 100f;

                //Update UI
                stockPrice_Text[i].text = "" + roundedCurrentValue;                
                if (dailyTickChange[i] >= 0)
                {
                    stockChange_Text[i].text = "+" + roundedTickChange;
                    stockChange_Text[i].color = green;
                }
                else
                {
                    stockChange_Text[i].text = "" + roundedTickChange;
                    stockChange_Text[i].color = red;
                }
                //draw Graph
                if (currentTick < maxTicks)
                {
                    float graphTmp1 = allWeekValues[i, currentTick - 1];
                    float graphTmp2 = allWeekValues[i, currentTick];

                    float differenceMagnitude = graphTmp2 - graphTmp1;


  
                    Vector3 differenceVector = new Vector3(1200 / maxTicks, (differenceMagnitude)*graphScale[i], 0f);

                    Image iffy = Instantiate(basicImage, graphs[i].transform);
                    iffy.rectTransform.sizeDelta = new Vector2((differenceVector.normalized.magnitude)*(1400/maxTicks), thicknessOfGraph);
                    iffy.rectTransform.pivot = new Vector2(0f, 0f);
                    iffy.rectTransform.localPosition = currentGraphPosition[i];
                    float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
                    iffy.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
                    currentGraphPosition[i] = new Vector3((1200 / maxTicks) * currentTick, (differenceMagnitude)*graphScale[i] + currentGraphPosition[i].y, 0f);
                }

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
        currentTick = 0;
        if (PlayerPrefs.GetInt("CurrentWeek") < gameTime)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                beginningOfDayValues[i] = currentValues[i];
                
                dailyTickChange[i] = (allValues[i, PlayerPrefs.GetInt("CurrentCycle") + 1] - currentValues[i]) / maxTicks; //median
                createCurve(i);
                maxWeekValue[i] = 0;
               
            }
        }
       
        
        for (int i = 0; i < numberOfGroups; i++)
        {
            maxWeekValue[i] = 0;
            graphScale[i] = 1;
            currentGraphPosition[i] = new Vector3(0f, 33.75f, 0f);
            for (int j = 0; j < maxTicks; j++)
            {
                float tmp = allWeekValues[i, j]- allValues[i, PlayerPrefs.GetInt("CurrentWeek")];
                if (tmp < 0) tmp *= -1;
                if (tmp > maxWeekValue[i])
                {
                    maxWeekValue[i] = tmp;  
                }
                
            }
            graphScale[i] = 32.5f / maxWeekValue[i];
        }

    }

    void EndWeek()
    {
        cycleOn = false;
        PlayerPrefs.SetInt("CurrentWeek", PlayerPrefs.GetInt("CurrentWeek") + 1);

        //UI
        weekStart_Text.text = "Week " + (PlayerPrefs.GetInt("CurrentWeek") + 1);
        weekEnd_Header_Text.text = "Week " + (PlayerPrefs.GetInt("CurrentWeek"));
        weekEnd_Earnings_Text.text = "You have earned " + PlayerPrefs.GetFloat("WeeklyEarnings") + "€ this week.\n\nYou spent " + PlayerPrefs.GetFloat("WeeklyExpenses") + "€.\n\nYour total balance is " + PlayerPrefs.GetFloat("CurrentBalance") + "€.";

        //Update Stats
        PlayerPrefs.SetFloat("CurrentBalance", PlayerPrefs.GetFloat("CurrentBalance ") + PlayerPrefs.GetFloat("WeeklyEarnings"));
        PlayerPrefs.SetFloat("WeeklyEarnings", 0);
        PlayerPrefs.SetFloat("WeeklyExpenses", 0);
    }

    void EndGame()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = 0;
            dailyTickChange[i] = 0;
        }
    }

    void createCurve(int group)
    {
        
        float totalChange = (allValues[group, PlayerPrefs.GetInt("CurrentCycle") + 1] / currentValues[group]) -1; //absolut
        int rando = Random.Range(0, 3);;
        Debug.Log(totalChange);
      
        if (totalChange <= -0.10f)
        {
            if (totalChange < 0) totalChange *= -1;
            switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = - 680 * (tmp) + 2873.333 * Mathf.Pow(tmp, 2) - 4320 * Mathf.Pow(tmp, 3) + 2026.667 * Mathf.Pow(tmp, 4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        ;
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = -633.3333 * (tmp) + 1760 * Mathf.Pow(tmp, 2) - 1866.667 * Mathf.Pow(tmp, 3) + 640 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:
                   

                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = +553.3333 * (tmp) - 3000 * Mathf.Pow(tmp, 2) + 4266.667 * Mathf.Pow(tmp, 3) - 1920 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                default:
                    break;
            }
            
           
        } //starker fall
        else if (totalChange > -0.10f && totalChange <= -0.05f)
        {
            if (totalChange < 0) totalChange *= -1;
            switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = 620 * (tmp) - 2853.333 * Mathf.Pow(tmp, 2) + 3840 * Mathf.Pow(tmp, 3) - 1706.667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);

                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = +553.3333 * (tmp) - 2520 * Mathf.Pow(tmp, 2) + 3146.667 * Mathf.Pow(tmp, 3) - 1280 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
                        allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:
                    

                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = +686.6667 * (tmp) - 2813.333 * Mathf.Pow(tmp, 2) + 3093.333 * Mathf.Pow(tmp, 3) - 1066.667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                default:
                    break;
            }
            
        } //mittlerer fall
        else if (totalChange > -0.05f && totalChange <= 0) //leichter fall
        {
            if (totalChange < 0) totalChange *= -1;

            switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = 633.3333 * (tmp) - 2333.333 * Mathf.Pow(tmp, 2)  + 2666.667 * Mathf.Pow(tmp, 3) - 1066.667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));

                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        ;

                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = -1146.667 * (tmp) + 5740 * Mathf.Pow(tmp, 2) - 8853.333 * Mathf.Pow(tmp, 3) + 4160 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:


                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = -6.666667 * (tmp) - 573.3333 * Mathf.Pow(tmp, 2) + 906.6667 * Mathf.Pow(tmp, 3) - 426.6667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                default:
                    break;
            }
        }
        else if (totalChange > 0f && totalChange <= 0.05f) //leicher Anstieg
        {
           
                switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = +513.3333 * (tmp) - 1960 * Mathf.Pow(tmp, 2) + 2826.667 * Mathf.Pow(tmp, 3) - 1280 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        ;

                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:

                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +486.6667 * (tmp) - 920 * Mathf.Pow(tmp, 2) + 1173.333 * Mathf.Pow(tmp, 3) - 640 *  Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;

                        double multiplikator = 300 * (tmp) + 506.6667 * Mathf.Pow(tmp, 2) + 320 * Mathf.Pow(tmp, 3) - 426.6667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;

            }
        }
        else if (totalChange > 0.05f && totalChange <= 0.1f) //mittlerer Anstieg
        {
            switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +1526.667 * (tmp) - 6013.333 * Mathf.Pow(tmp, 2) + 8213.333 * Mathf.Pow(tmp, 3) - 3626.667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));

                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        ;

                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:

                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +266.6667 * (tmp) - 1126.667  * Mathf.Pow(tmp, 2) + 2133.333 * Mathf.Pow(tmp, 3) - 1173.333 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = -13.33333 * (tmp) + 700 * Mathf.Pow(tmp, 2) - 1546.667 * Mathf.Pow(tmp, 3) + 960 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                default:
                    break;


            }
        }
        else if (totalChange > 0.1f)
        {
            switch (rando)
            {
                case 0:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +33.33333 * (tmp) + 2093.333 * Mathf.Pow(tmp, 2) - 4373.333 * Mathf.Pow(tmp, 3) + 2346.667 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);

                        ;
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 1:

                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +100 * (tmp) - 266.6667 * Mathf.Pow(tmp, 2) + 480 * Mathf.Pow(tmp, 3) - 213.3333 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));

                        allWeekValues[group, i] = (float)((totalChange) * (multiplikator / 100) * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                case 2:
                    for (int i = 0; i < maxTicks; i++)
                    {
                        float tmp = (float)(i + 1) / maxTicks;
                        double multiplikator = +466.6667 * (tmp) - 3940 * Mathf.Pow(tmp, 2) + 7733.333 * Mathf.Pow(tmp, 3) - 4160 * Mathf.Pow(tmp,4);
                        tmp = (float) (totalChange * (multiplikator / 100));
                        if (tmp < 0) tmp = 1 + tmp;
                        else if (tmp > 0) tmp = 1 + tmp;
                        else if (tmp == 0) tmp = 1;
allWeekValues[group, i] = (float)(Random.Range(-0.001f,0.001f)* allValues[group, PlayerPrefs.GetInt("CurrentWeek")] + tmp * allValues[group, PlayerPrefs.GetInt("CurrentWeek")]);
                        
                    }
                    allWeekValues[group, maxTicks - 1] = allValues[group, PlayerPrefs.GetInt("CurrentWeek") + 1];
                    break;
                default:
                    break;
            }
        }
        
    }
}
