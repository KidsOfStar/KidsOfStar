using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBase : UIBase
{
    // Top UI 전용 공통 처리
    public override void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    //public void ShowTopTitle(string title)
    //{
    //    Debug.Log($"Top UI Title: {title}");
    //    // 여기에 공통 타이틀 처리 가능
    //}
}
