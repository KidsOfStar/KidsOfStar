using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
    public List<Transform> parent;

    void Start()
    {
        //Managers.Instance.InitManagers();

        Managers.Instance.UIManager.Show<UITitle>();
    }

}
