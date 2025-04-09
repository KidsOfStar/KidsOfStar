using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public List<Transform> parent;

    void Start()
    {
        Managers.Instance.InitManagers();
        Managers.Instance.UIManager.Init();        // UIManager의 Canvas 및 Parent 자동 세팅
        Managers.Instance.UIManager.Show<UITitle>();
    }


}
