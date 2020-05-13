using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stock : MonoBehaviour
{
    public Sprite icon;
    public string name;
    public string id;

    public DetailView detailView;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { detailView.ShowStockDetail(this); });
    }
}
