using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private const float SmoothSpeed = 5f;
    [SerializeField] public Vector3 offset;

    public void SetTarget()
    {
        target = Managers.Instance.GameManager.Player.transform;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        #if UNITY_EDITOR
        if (Managers.Instance.IsDebugMode && !target)
            target = Managers.Instance.GameManager.Player.transform;
        #endif

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
