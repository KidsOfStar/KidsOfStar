using System;
using UnityEngine;

public class Chapter0503Base : SceneBase
{
    private bool cutSceneEnd = true;
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        SkillForm();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!cutSceneEnd)
            {
                // 컷신 호출
                Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.MeetingWomen);
            }
            cutSceneEnd = true;
        }
    }
}
