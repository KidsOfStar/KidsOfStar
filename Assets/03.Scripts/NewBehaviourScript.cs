using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            UnityGoogleSheet.LoadFromGoogle<int, MainTable.PlayerData>((list, map) => {
                if(list == null)
                {
                    Debug.Log("없음");
                }
                list.ForEach(x => {
                    Debug.Log(x.trustValue);
                });
            }, true);

    }


}
