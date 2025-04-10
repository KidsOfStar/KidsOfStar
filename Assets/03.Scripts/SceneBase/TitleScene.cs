using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        Managers.Instance.UIManager.Show<UITitle>();
    }
}
