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
        Managers.Instance.GameManager.ChapterProgress = 3;
        Debug.Log($"{Managers.Instance.GameManager.VisitCount}");
        Debug.Log($"{Managers.Instance.GameManager.ChapterProgress}");

        if (Managers.Instance.GameManager.VisitCount == 1 )
        {
            meetingWomanTriger.enabled = false;
            Managers.Instance.GameManager.ChapterProgress = 4;
        }
        else if (Managers.Instance.GameManager.VisitCount == 2)
        {
            meetingWomanTriger.enabled = false;
            meetingBihyiTriger.enabled = false;
        }
        Managers.Instance.GameManager.VisitCount++;


        if (Managers.Instance.GameManager.ChapterProgress == 4)
        {
            crowd.SetActive(false);
        }
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
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);
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
