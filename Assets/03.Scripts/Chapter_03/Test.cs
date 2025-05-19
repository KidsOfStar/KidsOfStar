using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private CutSceneType cutSceneType;
    [SerializeField] private BgmLayeredFader bgmFader;
    [SerializeField] private bool isTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTrigger) return;
        if (!other.gameObject.CompareTag("Player")) return;

        Managers.Instance.CutSceneManager.PlayCutScene(cutSceneType);
        if (bgmFader != null)
        {
            var bgm = Instantiate(bgmFader);
            bgm.Init();
        }
        
        isTrigger = false;
    }
}