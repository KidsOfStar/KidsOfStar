using System;
using UnityEngine.Rendering;


public class Chapter0501Base : SceneBase
{
    public bool istutorialForm = false;
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        EditorLog.Log("Chapter 5-1 시작");
        if (istutorialForm)
        {
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(3);
            istutorialForm = true;
            EditorLog.Log("튜토리얼 팝업이 열렸습니다.");
        }

        SkillForm();

    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Dog);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Cat);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }
    // 진행도를 저장하는 함수
}


