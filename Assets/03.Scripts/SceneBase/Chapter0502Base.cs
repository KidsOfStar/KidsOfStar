using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0502Base : SceneBase
{
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        SkillForm();
        
        Managers.Instance.GameManager.UpdateProgress(); // 진행도 2

    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }


}
