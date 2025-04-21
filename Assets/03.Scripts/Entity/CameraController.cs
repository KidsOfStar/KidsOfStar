using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new(0, 2f, -10f);
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    
    [Header("Light")] // 씬에 배치 할 라이트 오브젝트 : 컷씬 재생 중에는 비활성화
    [SerializeField] private GameObject lightObject;
    
    private Transform target = null;
    private const float SmoothSpeed = 5f;
    private CutSceneManager cutSceneManager;

    public void Init()
    {
        cutSceneManager = Managers.Instance.CutSceneManager;
        target = Managers.Instance.GameManager.Player.transform;
        
        if (!lightObject) return;
        cutSceneManager.OnCutSceneStart += () => lightObject.SetActive(false);
        cutSceneManager.OnCutSceneEnd += () => lightObject.SetActive(true);
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