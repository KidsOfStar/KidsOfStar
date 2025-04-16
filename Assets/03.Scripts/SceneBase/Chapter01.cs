using Cinemachine;
using System.Collections;
using UnityEngine;

public class Chapter01 : SceneBase
{
    protected override void Awake()
    {
        base.Awake();

        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;
        onLoadComplete?.Invoke();
        
        SpawnPlayer();
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Intro.GetName());
    }
}
