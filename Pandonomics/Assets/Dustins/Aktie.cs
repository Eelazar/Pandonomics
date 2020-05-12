using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Aktie : ScriptableObject
{
    public string stockName;
    private int gameTime = 13; //total gameDuration in Weeks 
    public float[] allStockValues; //manually inserted

    public void Awake()
    {
        allStockValues = new float[gameTime];
        for(float f = 0; f < gameTime; f++)
        {
            int i = (int)f;
            if (f < gameTime / 3)
            {
                allStockValues[i] = Random.Range(10f, 11f);
            }
            else if (f < gameTime / 1.5f)
            {
                allStockValues[i] = Random.Range(6f, 7f);
            }
            else if (f < gameTime)
            {
                allStockValues[i] = Random.Range(8f, 8.5f);

            }
        }
    }
}
