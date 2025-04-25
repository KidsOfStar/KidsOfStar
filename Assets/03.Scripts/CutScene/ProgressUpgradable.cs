using UnityEngine;

public class ProgressUpgradable : MonoBehaviour
{
    public void UpgradeProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
    }
}