using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new(0, 2f, -10f);
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    
    [Header("Light")]
    [SerializeField] private GameObject lightObject;
    
    private Transform target = null;
    private const float SmoothSpeed = 5f;
    private CutSceneManager cutSceneManager;

    public void Init()
    {
        cutSceneManager = Managers.Instance.CutSceneManager;
        cutSceneManager.OnCutSceneStart += () => lightObject.SetActive(false);
        cutSceneManager.OnCutSceneEnd += () => lightObject.SetActive(true);
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
        
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);
        transform.position = smoothedPosition;
    }
}