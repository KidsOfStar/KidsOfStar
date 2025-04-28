using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        playIntroCallback?.Invoke();

        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = skillBtn.skillUnlock;

        //skillUnlock.SetUnlockedSkills(Managers.Instance.GameManager.SavedUnlockedSkills);
        skillUnlock.ApplyUnlockedSkills();

        skillUnlock.UnlockSkill(1); // 다람쥐 스킬 잠금 해제
        skillUnlock.ShowSkillBG(skillUnlock.squirrelBG); // 다람쥐 스킬 BG 활성화

        //Managers.Instance.GameManager.SavedUnlockedSkills = skillUnlock.GetUnlockedSkills();
    }
}
