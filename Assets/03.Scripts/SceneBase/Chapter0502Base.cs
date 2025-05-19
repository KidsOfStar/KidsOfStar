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
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        Managers.Instance.GameManager.UpdateProgress(); // 진행도 2
    }
}
