using System;
using System.Collections;
using UnityEngine;

public class Chapter04Base : SceneBase
{
    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.GameManager.OnProgressUpdated += AddListenerTutorial;
        callback?.Invoke();
    }
    
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.City);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.City);
    }

    private void AddListenerTutorial()
    {
        if (!isFirstTime) return;

        if (Managers.Instance.GameManager.ChapterProgress == 2)
        {
            StartCoroutine(CatTutorial());
        }
    }

    private IEnumerator CatTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        
        var tutorial = Managers.Instance.UIManager.Show<UITutorial>();
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var catBtn = skillPanel.catBtn.GetComponent<RectTransform>();
        tutorial.SetTarget(catBtn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Managers.Instance.GameManager.OnProgressUpdated -= AddListenerTutorial;
    }
}
