using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneLode : MonoBehaviour
{
    public SceneType sceneType;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // CutSceneLode 투명화하기
            Managers.Instance.SceneLoadManager.LoadScene(sceneType);
        }
    }
}
