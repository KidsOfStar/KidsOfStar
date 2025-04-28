using System;
using UnityEngine;

// SceneBase에서 Start, Awake 쓰지 말아주세요 순서 꼬여요
// InitSceneExtra에서 처리해주세요
// 씬 초기화에 필요한 모든 작업 이후 자동으로 InitSceneExtra가 호출됩니다.
public class Chapter03Base : SceneBase
{
    [Header("Chapter 3")]
    [SerializeField] private DashGame dashGame;

    protected override void InitSceneExtra(Action playIntroCallback)
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        //var skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;
        var skillUnlock = skillBtn.skillUnlock;

        skillUnlock.SetUnlockedSkills(Managers.Instance.GameManager.SavedUnlockedSkills);
        skillUnlock.ApplyUnlockedSkills();

        // 강아지 스킬 잠금 해제
        skillUnlock.UnlockSkill(2); // 다람쥐 스킬 잠금 해제
        skillUnlock.ShowSkillBG(skillUnlock.dogBG); // 강아지 스킬 BG 활성화

        Managers.Instance.GameManager.SavedUnlockedSkills = skillUnlock.GetUnlockedSkills();

    }
}