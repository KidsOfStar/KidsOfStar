using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public List<Transform> parent;

    void Start()
    {
        Managers.Instance.OnSceneLoaded();

        Managers.Instance.UIManager.Show<UITitle>();
    }
}
