using System;
using UnityEngine;

// SceneBase에서 Start, Awake 쓰지 말아주세요 순서 꼬여요
// InitSceneExtra에서 처리해주세요
// 씬 초기화에 필요한 모든 작업 이후 자동으로 InitSceneExtra가 호출됩니다.
public class Chapter03Base : SceneBase
{
    [Header("Chapter 3")]
    [SerializeField] private DashGame dashGame;

    protected override void InitSceneExtra(Action playIntroCallback)
    {
        // 강아지 스킬 잠금 해제
        Managers.Instance.UIManager.SkillUnlock.UnlockSkill(2);
    }
}
