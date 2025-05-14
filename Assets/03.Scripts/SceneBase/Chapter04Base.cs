using System;
using UnityEngine;

public class Chapter04Base : SceneBase
{
    protected override void InitSceneExtra(Action callback)
    {
        callback?.Invoke();
    }
    
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro(CatTutorial);
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.City);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.City);
    }

    private void CatTutorial()
    {
        var tutorial = Managers.Instance.UIManager.Show<UITutorial>();
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var catBtn = skillPanel.catBtn.GetComponent<RectTransform>();
        tutorial.SetTarget(catBtn);
    }
}
