using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        SkillForm();
        playIntroCallback?.Invoke();
    }

    // 씬이 로드되자마자 재생되는 컷신이 있다면 이 곳에 컷신이 끝났을 때 호출 될 콜백을 작성합니다.
    protected override void ChapterCutSceneCallback()
    {
        PlayChapterIntro();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.ForestBird);
    }

    private void SkillForm()
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = skillBtn.skillUnlock;

        Managers.Instance.GameManager.UnlockForm("Squirrel");
        skillUnlock.UnlockSkill("Squirrel");
        skillUnlock.ShowSkillBG(skillUnlock.squirrelBG); // 'Squirrel' 스킬 BG 활성화
    }
}
