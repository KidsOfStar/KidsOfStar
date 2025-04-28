using System;
using UnityEngine;

public class Chapter03Base : SceneBase
{
    [Header("Chapter 3")]
    [SerializeField] private DashGame dashGame;

    protected override void InitSceneExtra(Action playIntroCallback)
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        //var skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;
        var skillUnlock = skillBtn.skillUnlock;


        // skillUnlock.SetUnlockedSkills(Managers.Instance.GameManager.SavedUnlockedSkills);
        // skillUnlock.ApplyUnlockedSkills();
        // 
        // // 강아지 스킬 잠금 해제
        //Managers.Instance.UIManager.SkillUnlock.UnlockSkill(1);
        skillUnlock.UnlockSkill("Squirrel"); // 강아지 스킬 잠금 해제
        skillUnlock.UnlockSkill("Dog"); // 강아지 스킬 잠금 해제
        Managers.Instance.UIManager.SkillUnlock.ShowSkillBG(skillUnlock.dogBG); // 강아지 스킬 BG 활성화

        //Managers.Instance.GameManager.SavedUnlockedSkills = skillUnlock.GetUnlockedSkills();

        // 


    }
}