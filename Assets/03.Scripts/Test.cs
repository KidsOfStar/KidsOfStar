using MainTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private List<PlayerData> playerdata;
    private Dictionary<int, PlayerData> playerDataMap;
    private void Awake()
    {
        playerdata = PlayerData.GetList();
        playerDataMap = PlayerData.GetDictionary();

    }
    private void Start()
    {
       foreach(var value in playerdata)
        {
            Debug.Log(value.index.ToString());
        }
    }
}
