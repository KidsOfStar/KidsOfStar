using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0503Base : SceneBase
{
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
            // 컷신 호출
            //Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.MeetingWomen);
        }
    }


}
