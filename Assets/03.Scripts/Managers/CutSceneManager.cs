using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutSceneManager
{
    private readonly UnityEvent showDialogEvent = new();
    private CutSceneBase cutSceneBase;

    [Header("CutSceneData")]
    private CutSceneData cutSceneData;
    public bool IsCutScenePlaying { get; private set; } = false;
    public Action OnCutSceneEnd { get; set; }
    private LetterBoxer letterBoxer;
    public LetterBoxer LetterBoxer { get { return letterBoxer; } }

    public void SetCutSceneBase(CutSceneBase cutScene)
    {
        this.cutSceneBase = cutScene;
        OnCutSceneEnd += () => IsCutScenePlaying = false;
    }
    
    public void PlayCutScene(string cutsceneName)
    {
        //letterBoxer = Managers.Instance.GameManager.MainCamera.GetComponent<LetterBoxer>();
        string prefabPath = $"CutScenes/{cutsceneName}";
        var prefab = Resources.Load<GameObject>(prefabPath);
        //letterBoxer.PerformSizing();

        if (prefab == null)
        {
            EditorLog.Log($"컷씬 프리팹이 없습니다: {prefabPath}");
            return;
        }

        var instance = GameObject.Instantiate(prefab);
        

        // CutSceneBase 찾기 및 등록
        var baseComp = instance.GetComponentInChildren<CutSceneBase>();
        if (baseComp != null)
        {
            SetCutSceneBase(baseComp);
        }

        // PlayableDirector 찾기 및 등록
        var director = baseComp.Director;
        //director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        director.Play();
        IsCutScenePlaying = true;
    }
}
