using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public List<Transform> parent;


    // Start is called before the first frame update
    void Start()
    {
        Managers.Instance.UIManager.SetParents(parent);
        Managers.Instance.UIManager.Show<UITitle>();
        Managers.Instance.GameManager.Init();
        Managers.Instance.SoundManager.Init();
    }


}
