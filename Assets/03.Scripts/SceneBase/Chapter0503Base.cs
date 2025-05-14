using System;
using UnityEngine;

public class Chapter0503Base : SceneBase
{
    public Collider2D meetingWomanTriger;   // 앞
    public Collider2D meetingBihyiTriger;   // 뒤

    private void Start()
    {
        if(Managers.Instance.GameManager.visitCount == 1 )
        {
            meetingWomanTriger.enabled = false;
        }
        else if (Managers.Instance.GameManager.visitCount == 2)
        {
            meetingWomanTriger.enabled = false;
            meetingBihyiTriger.enabled = false;
        }
        Managers.Instance.GameManager.visitCount++;
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
}
