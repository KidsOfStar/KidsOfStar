using UnityEngine;

public class Chapter01Base : SceneBase
{
    [Header("Chapter 1")]
    [SerializeField] private SceneEventTrigger sceneEventTrigger;
    
    protected override void InitSceneExtra()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Maorum);
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Intro.GetName());
        sceneEventTrigger.Init();
    }
}