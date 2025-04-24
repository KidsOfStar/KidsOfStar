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

    }

    private void Start()
    {
        //dashGame = GetComponent<DashGame>();
        //Game();
    }

    private void Game()
    {
        // 챕터 진행도 2일 때
        if(Managers.Instance.GameManager.ChapterProgress == 2)
        {
            // DashGame 보이기
            dashGame.gameObject.SetActive(true);
        }
        else
        {
            // DashGame 종료
            dashGame.gameObject.SetActive(true);

        }
    }

}
