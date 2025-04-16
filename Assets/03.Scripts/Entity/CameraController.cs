using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target = null;
    private const float SmoothSpeed = 5f;
    [SerializeField] public Vector3 offset;

    public void SetTarget()
    {
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

        if (!target) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}