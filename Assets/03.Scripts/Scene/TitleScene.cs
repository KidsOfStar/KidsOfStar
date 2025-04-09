using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public List<Transform> parent;

    void Start()
    {
        Managers.Instance.UIManager.SetParents(parent);
        Managers.Instance.UIManager.Show<UITitle>();
        //Managers.Instance.InitManagers();
    }


}
