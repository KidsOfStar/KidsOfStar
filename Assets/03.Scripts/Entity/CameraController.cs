using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Vector3 offset = new(0, 2f, -10f);
    private Transform target = null;
    private const float SmoothSpeed = 5f;
    private CutSceneManager cutSceneManager;

    public void SetTarget()
    {
        cutSceneManager = Managers.Instance.CutSceneManager;
        target = Managers.Instance.GameManager.Player.transform;
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Managers.Instance.IsDebugMode && !target)
        {
            target = FindObjectOfType<Player>().transform;
        }
#endif

        if (!target || cutSceneManager.IsCutScenePlaying) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}