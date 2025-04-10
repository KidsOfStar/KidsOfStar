using UnityEngine;

public class GameManager : ISceneLifecycleHandler
{
    // TODO: 스테이지 진행사항 (저장 및 NPC 대사 변화를 위해)

    public Camera MainCamera { get; private set; }
    public float SfxVolume { get; private set; }
    public float BgmVolume { get; private set; }

    public GameManager()
    {
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

    public void OnSceneLoaded()
    {
        MainCamera = Camera.main;
    }
    
    public void OnSceneUnloaded()
    {
        MainCamera = null;
    }
}
