using System;
using UnityEngine;

public class Chapter02Base : SceneBase
{
    [Header("Chapter 2")]
    public SkillBTN skillBTN;
    public SkillUnlock skillUnlock;

    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        playIntroCallback?.Invoke();

        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;

        skillUnlock.UnlockSkill(1); // 강아지 스킬 잠금 해제
    }
    private void Start()
    {
        
    }
}
