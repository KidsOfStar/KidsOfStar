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
    public LetterBoxer LetterBoxer { get; private set; }
    
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
    
    public void PlayCutScene(string cutsceneName, Action localEndCallback = null)
    {
        CurrentCutSceneName = cutsceneName;

        //letterBoxer = Managers.Instance.GameManager.MainCamera.GetComponent<LetterBoxer>();
        string prefabPath = $"{CutScenePath}{cutsceneName}";
        var cutSceneBase = Managers.Instance.ResourceManager.Instantiate<CutSceneBase>(prefabPath);
        //letterBoxer.PerformSizing();

        if (!cutSceneBase)
        {
            EditorLog.Log($"컷씬 프리팹이 없습니다: {prefabPath}");
            return;
        }
        
        if (localEndCallback != null) cutSceneBase.OnCutSceneCompleted += localEndCallback;
        cutSceneBase.Init();
        cutSceneBase.Play();

        // PlayableDirector 찾기 및 등록
        // baseComp.Play
        OnCutSceneStart?.Invoke();
        IsCutScenePlaying = true;
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
