using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    // TODO: 스테이지 진행사항 (저장 및 NPC 대사 변화를 위해)

    // Settings
    public Camera MainCamera { get; private set; }
    public float SfxVolume { get; private set; }
    public float BgmVolume { get; private set; }

    // Stage Data
    private readonly Dictionary<ChapterType, int> trustDict = new();
    private readonly Dictionary<EndingType, bool> endingDict = new();
    public ChapterType CurrentChapter { get; private set; }
    public int ChapterProgress { get; private set; }
    
    
    // Play Data
    public Player Player { get; private set; }

    public GameManager()
    {
#if UNITY_WEBGL
        Application.targetFrameRate = 60;
#elif UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = 60;
#endif
        InitDictionary();
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

    // TODO: 이어하기 라면 로드 데이터에서 가져오기
    private void InitDictionary()
    {
        var count = Enum.GetValues(typeof(ChapterType)).Length;
        for (int i = 0; i < count; i++)
        {
            var chapter = (ChapterType)i;
            trustDict.TryAdd(chapter, 0);
        }
        
        count = Enum.GetValues(typeof(EndingType)).Length;
        for (int i = 0; i < count; i++)
        {
            var ending = (EndingType)i;
            endingDict.TryAdd(ending, false);
        }
    }

    public int[] GetTrustArray()
    {
        var count = Enum.GetValues(typeof(ChapterType)).Length;
        var trustArr = new int[count];
        for (int i = 0; i < count; i++)
        {
            var chapter = (ChapterType)i;
            trustArr[i] = trustDict[chapter];
        }
        
        return trustArr;
    }
    
    public bool[] GetEndingArray()
    {
        var count = Enum.GetValues(typeof(EndingType)).Length;
        var endingArr = new bool[count];
        for (int i = 0; i < count; i++)
        {
            var ending = (EndingType)i;
            endingArr[i] = endingDict[ending];
        }
        
        return endingArr;
    }
    
    public void ModifyTrust(int value)
    {
        trustDict[CurrentChapter] += value;
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