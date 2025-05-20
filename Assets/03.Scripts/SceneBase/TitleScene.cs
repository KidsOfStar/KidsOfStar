using UnityEngine;

public class TitleScene : MonoBehaviour
{
    protected void Start()
    {
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;
        Managers.Instance.UIManager.Show<BackGroundTitile>();
        Managers.Instance.AnalyticsManager.SendFunnel("1");
    }
}