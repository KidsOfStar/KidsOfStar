using System;
using UnityEngine.Rendering;


public class Chapter0501Base : SceneBase
{
    public bool istutorialForm = false;
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro(HideTutorial);
    }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);
        callback?.Invoke();
    }

    private void HideTutorial()
    {
        if (istutorialForm)
        {
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(3);
            istutorialForm = true;
        }
    }
}


