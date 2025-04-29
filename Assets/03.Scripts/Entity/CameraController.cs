using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool followTarget = true;
    [SerializeField] private Vector3 offset = new(0, 2f, -10f);
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    
    [Header("Light")] // 씬에 배치 할 라이트 오브젝트 : 컷씬 재생 중에는 비활성화
    [SerializeField] private GameObject lightObject;
    
    private Transform target = null;
    private CutSceneManager cutSceneManager;
    private const float SmoothSpeed = 8f;

    public void Init()
    {
        cutSceneManager = Managers.Instance.CutSceneManager;
        target = Managers.Instance.GameManager.Player.transform;

        if (lightObject)
        {
            cutSceneManager.OnCutSceneStart += InactivateLight;
            cutSceneManager.OnCutSceneEnd += ActiveLight;
        }
    }

    private void FixedUpdate()
    {
        if (!followTarget)
            return;
        
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
        // desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        // desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
        transform.position = smoothedPosition;
    }
    
    private void ActiveLight()
    {
        if (!lightObject) return;
        lightObject.SetActive(true);
    }
    
    private void InactivateLight()
    {
        if (!lightObject) return;
        lightObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        cutSceneManager.OnCutSceneStart -= InactivateLight;
        cutSceneManager.OnCutSceneEnd -= ActiveLight;
    }
}