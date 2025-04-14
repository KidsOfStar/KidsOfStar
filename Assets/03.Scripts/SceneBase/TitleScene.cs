using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    protected void Start()
    {
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = false;
        Managers.Instance.UIManager.Show<UITitle>();
    }
}