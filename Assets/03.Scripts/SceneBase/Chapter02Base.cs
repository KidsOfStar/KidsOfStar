using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        playIntroCallback?.Invoke();

        Managers.Instance.GameManager.UnlockForm(playerStartForm);
        var skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;

        skillUnlock.UnlockSkill(1); // 다람쥐 스킬 잠금 해제
        Managers.Instance.UIManager.SkillUnlock.UnlockSkill(1); // 다람쥐 스킬 잠금 해제
    }
}
