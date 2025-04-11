using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneManager : ISceneLifecycleHandler
{
    private PlayableDirector director;
    private List<CinemachineVirtualCamera> vCams = new();

    private Dictionary<string, PlayableAsset> cutsceneTable = new();
    private Dictionary<string, string> sceneToCutscene = new();

    private HashSet<string> playedCutscenes = new();

    // Director를 연결해주는 메서드
    public void SetDirector(PlayableDirector pd)
    {
        director = pd;
    }

    // 카메라를 설정하는 메서드
    public void SetVirtualCam(IEnumerable<CinemachineVirtualCamera> cameras)
    {
        vCams.Clear();
        vCams.AddRange(cameras);  // 리스트에 여러개의 값을 한번에 추가
    }

    // 컷씬을 등록해주는 메서드 
    public void RegisterCutScene(string name, PlayableAsset asset)
    {
        cutsceneTable[name] = asset;
    }

    // 
    public void MapSceneToCutScene(string sceneName, string cutsceneName)
    {
        sceneToCutscene[sceneName] = cutsceneName;
    }

    public void PlayCutScene(string cutsceneName, Action onEnd = null)
    {
        if(director == null)
        {
            EditorLog.Log("Playable Director is not assigned");
            return;
        }
        if(!cutsceneTable.TryGetValue(cutsceneName, out var asset))
        {
            EditorLog.Log($"CutScene {cutsceneName} is not assigned");
            return;
        }

        director.playableAsset = asset;
        director.Play(asset);

        if(onEnd != null)
        {
            director.stopped += HandleOnCutsceneEnd;
        }

        void HandleOnCutsceneEnd(PlayableDirector pd) 
        {
            onEnd?.Invoke();
            director.stopped -= HandleOnCutsceneEnd;
        }
    }

    private void DeactivateAllvCam()
    {
        foreach(var vcam in vCams)
        {
            vcam.gameObject.SetActive(false);
        }
    }

    public void OnSceneLoaded()
    {
        DeactivateAllvCam();

        string sceneName = Managers.Instance.SceneLoadManager.CurrentScene.GetName();

        if (sceneToCutscene.TryGetValue(sceneName, out string cutsceneName))
        {
            if (playedCutscenes.Contains(cutsceneName))
                return;
        }
        PlayCutScene(cutsceneName, () =>
        {
            playedCutscenes.Add(cutsceneName);
        });
    }

    public void OnSceneUnloaded()
    {
        DeactivateAllvCam();
        director = null;
        vCams.Clear();
    }

    public bool HasMappedCutScene(string sceneName, out string cutsceneName)
    {
        return sceneToCutscene.TryGetValue(sceneName, out cutsceneName);
    }

}
