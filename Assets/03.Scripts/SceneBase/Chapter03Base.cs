
using System;

public class Chapter03Base : SceneBase
{
    private DashGame dashGame;
    protected override void InitSceneExtra(Action playIntroCallback)
    {

    }

    private void Start()
    {
        dashGame = GetComponent<DashGame>();
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
