using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter02 : MonoBehaviour
{

    private void Start()
    {
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Chapter02.GetName());
    }
}
