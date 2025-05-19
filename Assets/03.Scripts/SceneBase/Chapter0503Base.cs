using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0503Base : SceneBase
{
    [Header("Chapter 0503")]
    public GameObject crowd;
    public Collider2D meetingWomanTriger;   // 앞
    public Collider2D meetingBihyiTriger;   // 뒤

    private void Start()
    {
        var gm = Managers.Instance.GameManager;

        Debug.Log($"[Chapter503] VisitCount: {gm.VisitCount}, ChapterProgress: {gm.ChapterProgress}");

        gm.ChapterProgress = 3;

        if (gm.VisitCount == 0)
        {
            // 처음 503 진입
            meetingWomanTriger.enabled = true;
            meetingBihyiTriger.enabled = false;
            gm.VisitCount++;
        }
        else if (gm.VisitCount == 1)
        {
            // 두 번째 503 진입 (504 -> 503)
            meetingWomanTriger.enabled = false;
            meetingBihyiTriger.enabled = true;
            gm.ChapterProgress = 4;
        }


        // Crowd 처리
        if (gm.ChapterProgress == 4)
            crowd.SetActive(false);

        Debug.Log($"[Chapter503] 최종 VisitCount: {gm.VisitCount}");
    }


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
        //Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Dog);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Cat);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var upgrade = Managers.Instance.GameManager;
            if (upgrade.ChapterProgress == 4)
            {
                upgrade.UpdateProgress();
            }
        }
    }
}
