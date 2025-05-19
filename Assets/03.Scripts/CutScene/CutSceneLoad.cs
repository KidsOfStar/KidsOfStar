using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneLoad : MonoBehaviour
{
    public CutSceneType cutSceneType;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.Instance.CutSceneManager.PlayCutScene(cutSceneType);
        }
    }
}
