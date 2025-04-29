using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        playIntroCallback?.Invoke();

        SkillForm();

    }
    private void SkillForm()
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = skillBtn.skillUnlock;

        Managers.Instance.GameManager.UnlockForm("Squirrel");
        skillUnlock.UnlockSkill("Squirrel");
        //skillUnlock.ShowSkillBG(skillUnlock.squirrelBG); // 'Squirrel' 스킬 BG 활성화

    }
}
