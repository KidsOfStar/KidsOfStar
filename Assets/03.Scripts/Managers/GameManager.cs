using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // TODO: 스테이지 진행사항 (저장 및 NPC 대사 변화를 위해)
    
    // Settings
    public Camera MainCamera { get; private set; }
    public float SfxVolume { get; private set; }
    public float BgmVolume { get; private set; }
    
    // Play Data
    private Dictionary<ChapterType, int> trustDict = new();
    public ChapterType currentChapter { get; private set; } // TODO: 챕터별 진행사항?
    public Player Player { get; private set; }

    public GameManager()
    {
#if UNITY_WEBGL
        Application.targetFrameRate = 60;
#elif UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = 60;
#endif
        LoadVolumeSetting();
    }

    public void SaveVolumeSetting()
    {
        PlayerPrefs.SetFloat("BgmVolume", BgmVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
    }

    public void LoadVolumeSetting()
    {
        BgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0.7f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.7f);
    }

    public void DeleteVolumeSetting()
    {
        PlayerPrefs.DeleteKey("BgmVolume");
        PlayerPrefs.DeleteKey("SfxVolume");
    }

    public void SetCamera(Camera camera)
    {
        MainCamera = camera;
    }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
}