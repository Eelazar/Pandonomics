using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerPandonomics : MonoBehaviour
{
    public float[] startingRandomValues; //random values for the stockgroups
    public int maxTicks; //ticks per week
    public int currentWeek; //duh
    public int currentTick; //currenttick of the gametimeunit
    public AktienGruppe[] allStockGroups; //all of the groups
    public int numberOfGroups; //how much different groups are in the game
    public TextMeshProUGUI[] textStockGroups; //testtexte
    public int gameTime; //complete duration of the game in weeks
    public float[,] allValues; //first Value is the Stock Group, Second the current week;
    public float[] beginningOfDayValues; //wert am anfang des tages
    public float[] dailyTickChange; //calculated change per tick
    public float[] currentValues; //all current values
    void Start()
    {
        numberOfGroups = allStockGroups.Length;
        startingRandomValues = new float[numberOfGroups];
        allValues = new float[numberOfGroups,gameTime+1];
        dailyTickChange = new float[numberOfGroups];
        beginningOfDayValues = new float[numberOfGroups];
        currentValues = new float[numberOfGroups];


        for (int i = 0; i < numberOfGroups; i++)
        {
            
            allStockGroups[i].calculateGrowth();
            startingRandomValues[i] = Random.Range(47.8f, 512.6f);
            allValues[i,0] = startingRandomValues[i];
            
        }
        for(int i = 0; i < gameTime; i++)
        {
            
            for(int j = 0; j < numberOfGroups; j++)
            {
                
                allValues[j, i + 1] = allValues[j, i] * (1 + allStockGroups[j].allGrowthValues[i]);
            }
        }
        startGame();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { tick();

            for (int i = 0; i < gameTime;i++)
            {
                Debug.Log(allValues[2, i]);
            }
        }
    }

    void startGame()
    {
        currentWeek = 0;
        for(int i = 0; i < textStockGroups.Length; i++)
        {
            textStockGroups[i].text = "" +allValues[i, 0];
        }
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = allValues[i, 0];

        }
        startWeek();
    }

    void tick()
    {
        currentTick++;
        if (currentTick <= maxTicks)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                currentValues[i] += dailyTickChange[i];
                textStockGroups[i].text = "" + currentValues[i];
            }
        }
        if (currentTick >= maxTicks)
        {
            endWeek();
        }
    }

    void startWeek()
    {
        currentTick = 0;
        if (currentWeek < gameTime)
        {
            for (int i = 0; i < numberOfGroups; i++)
            {
                beginningOfDayValues[i] = currentValues[i];
                dailyTickChange[i] = (allValues[i, currentWeek+1] - currentValues[i]) / maxTicks;
            }
        }

    }

    void endWeek()
    {
        currentTick=0;
        currentWeek++;
        if (currentWeek <= gameTime) startWeek();
        else if (currentWeek >= gameTime) endGame();
    }

    void endGame()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            currentValues[i] = 0;
            dailyTickChange[i] = 0;
        }
    }
}
