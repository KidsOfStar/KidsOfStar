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
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        var skillUnlock = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel.skillUnlock;

        // 강아지 스킬 잠금 해제
        Managers.Instance.UIManager.SkillUnlock.UnlockSkill(1);
        Managers.Instance.UIManager.SkillUnlock.UnlockSkill(2);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DashGame(); // DashGame 시작
        }
    }

    private void DashGame()
    {
        // 챕터 진행도 2일 때
        if (Managers.Instance.GameManager.ChapterProgress == 2)
        {
            // DashGame 시작
            dashGame.StartGame();
        }
    }
}