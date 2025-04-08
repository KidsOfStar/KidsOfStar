using UnityEngine;

public class GameManager
{
    // 스테이지 진행사항 (저장 및 NPC 대사 변화를 위해)

    public Camera mainCamera { get; private set; }

    public void Init()
    {
        mainCamera = Camera.main;
    }
}
