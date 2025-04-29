using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        playIntroCallback?.Invoke();

        //Managers.Instance.GameManager.UnlockedForms("Squirrel"); // 다람쥐 스킬 잠금 해제    

        var skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;

        skillUnlock.UnlockSkill("Squirrel"); // 강아지 스킬 잠금 해제
        skillUnlock.ShowSkillBG(skillUnlock.squirrelBG); // 다람쥐 스킬 BG 활성화
        //skillUnlock
    }
}
