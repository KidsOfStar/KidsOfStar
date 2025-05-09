using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0503Base : SceneBase
{
    protected override void ChapterCutSceneCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {

    }
}
