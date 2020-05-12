using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class AktienGruppe : ScriptableObject
{
    private int gameTime = 13; //total gameDuration in Weeks anmerkung an mich: hol die variable vom gamemanager
    public string stockGroupName; //Obvious
    public float[] allGrowthValues; //calculated. index 0 is from start of game to week 1
    public int numberOfStocks; //number of Stocks in the Group
    public Aktie[] allStocks; //arrayofallstocks in the group
    public void Awake()
    {
        allGrowthValues = new float[gameTime-1];
    }
    public void calculateGrowth()
    {
        
        numberOfStocks = allStocks.Length;
        float[] tmpStockGrowth = new float[numberOfStocks];
        float sumOfAllStockGrowths;
        for(int i = 0; i < gameTime - 1; i++)
        {
            
            sumOfAllStockGrowths = 0;
            for (int j = 0; j < numberOfStocks; j++)
            {

                tmpStockGrowth[j] = allStocks[j].allStockValues[i + 1] / allStocks[j].allStockValues[i];

            }
            for(int j = 0; j < numberOfStocks; j++)
            {
                sumOfAllStockGrowths += tmpStockGrowth[j];
            }

            allGrowthValues[i] = (sumOfAllStockGrowths / numberOfStocks)-1;
        }

    }
}
