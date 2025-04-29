using System;
using Unity.VisualScripting;
using UnityEngine;

public class Chapter03Base : SceneBase
{
    private DashGame dashGame;
    private DashInteractable dashInteractable;
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        dashGame = FindObjectOfType<DashGame>();
        dashInteractable = FindObjectOfType<DashInteractable>();

        Init();
        SkillForm();

    }

    private void Init()
    {

    }

    private void SkillForm()
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = skillBtn.skillUnlock;

        // GameManager에 'Dog' 스킬 해금 기록
        Managers.Instance.GameManager.UnlockForm("Dog");

        // SkillUnlock이 'Dog' 스킬을 UI에 반영하도록 호출
        skillUnlock.UnlockSkill("Dog");
        //skillUnlock.ShowSkillBG(skillUnlock.dogBG); // 'Dog' 스킬 BG 활성화
    }
}