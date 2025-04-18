using Cinemachine;
using System.Collections;
using UnityEngine;

public class Chapter01 : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;

        SpawnPlayer();
        onLoadComplete?.Invoke();
        
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Chapter02_Test.GetName());
    }
}

