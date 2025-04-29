using System;
using UnityEngine;

public class Chapter03Base : SceneBase
{
    [SerializeField] private DashGame dashGame;
    [SerializeField] private DashInteractable[] dashInteractable;
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Init();
        SkillForm();

    }

    private void Init()
    {
        dashGame.Setting();

        for(int i = 0; i < dashInteractable.Length; i++)
        {
            dashInteractable[i].Init();
        }
    }

    private void SkillForm()
    {
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = skillBtn.skillUnlock;

        // GameManager에 'Dog' 스킬 해금 기록
        Managers.Instance.GameManager.UnlockForm("Dog");

        // SkillUnlock이 'Dog' 스킬을 UI에 반영하도록 호출
        skillUnlock.UnlockSkill("Dog");
        skillUnlock.ShowSkillBG(skillUnlock.dogBG); // 'Dog' 스킬 BG 활성화
    }
}