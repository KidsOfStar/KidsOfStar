using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTitile : UIBase
{
    // Start is called before the first frame update
    void Start()
    {
        // 현재 경로 
        Managers.Instance.UIManager.Show<UITitle>();
    }
}
