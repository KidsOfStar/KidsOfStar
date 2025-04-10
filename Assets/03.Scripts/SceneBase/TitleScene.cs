using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
    void Start()
    {
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.UIManager.Show<UITitle>();
    }
}
