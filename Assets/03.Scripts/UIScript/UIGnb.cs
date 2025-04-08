using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGnb : UIBase
{
    


    public void OnClickEndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#else
        // 게임 종료
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }
}
