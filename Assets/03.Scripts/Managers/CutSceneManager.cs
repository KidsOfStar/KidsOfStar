using System;
using UnityEngine;

public class CutSceneManager
{
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerSpawnPos { get; private set; }
    public string CurrentCutSceneName { get; private set; }

    public bool IsCutScenePlaying { get; private set; } = false;
    public Action OnCutSceneStart { get; set; }
    public Action OnCutSceneEnd { get; set; }
    // public LetterBoxer LetterBoxer { get; private set; }
    
    private CutSceneBase currentCutScene;
    private const string CutScenePath = "CutScenes/";

    public CutSceneManager()
    {
        OnCutSceneEnd += () =>
        {
            IsCutScenePlaying = false;
            currentCutScene = null;
        };
    }
    
    public void PlayCutScene(CutSceneType cutscene, Action localEndCallback = null)
    {
        CurrentCutSceneName = cutscene.GetName();
        // letterBoxer = Managers.Instance.GameManager.MainCamera.GetComponent<LetterBoxer>();
        string prefabPath = $"{CutScenePath}{cutscene.GetName()}";
        var cutSceneBase = Managers.Instance.ResourceManager.Instantiate<CutSceneBase>(prefabPath);
        
        if (!cutSceneBase)
        {
            EditorLog.Log($"컷씬 프리팹이 없습니다: {prefabPath}");
            return;
        }
        
        // letterBoxer.PerformSizing();
        currentCutScene = cutSceneBase;
        IsCutScenePlaying = true;
        cutSceneBase.Play();
        OnCutSceneStart?.Invoke();
        
        if (localEndCallback != null) cutSceneBase.OnCutSceneCompleted += localEndCallback;
        cutSceneBase.Init();
    }

    public bool IsPlayingCutScene()
    {
        return currentCutScene != null;
    }
    
    public void DestroyCurrentCutScene()
    {
        if (!currentCutScene)
            EditorLog.LogError("현재 재생중인 컷씬이 없습니다.");

        currentCutScene.DestroyPrefab(true);
    }
    public void SetPlayerReferences(Transform player, Transform spawnPos)
    {
        PlayerTransform = player;
        PlayerSpawnPos = spawnPos;
    }
}
