using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0504Base : SceneBase
{
    protected override void CutSceneEndCallback()
    {
        // 컷신이 끝났을 때 호출되는 콜백
        PlayChapterIntro();
    }
    protected override void InitSceneExtra(System.Action callback)
    {
        SkillForm();
    }
    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }



}
