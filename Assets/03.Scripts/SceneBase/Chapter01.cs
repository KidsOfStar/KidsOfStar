using System;
using UnityEngine;

public class Chapter01 : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title);
        }
    }
}
