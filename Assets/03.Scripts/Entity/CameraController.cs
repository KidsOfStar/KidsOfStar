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
    [SerializeField] private Transform target = null;
    
    [Header("Dialog Camera")]
    [SerializeField] private GameObject dialogCam;
    
    private const float SmoothSpeed = 5f;
    private CutSceneManager cutSceneManager;

    public void Init()
    {
        cutSceneManager = Managers.Instance.CutSceneManager;
        target = Managers.Instance.GameManager.Player.transform;
        
        if (!lightObject) return;
        cutSceneManager.OnCutSceneStart += InactivateLight;
        cutSceneManager.OnCutSceneEnd += ActiveLight;
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
    
    private void ActiveDialogCam()
    {
        if (Managers.Instance.CutSceneManager.IsCutScenePlaying) return;
        dialogCam.SetActive(true);
    }
    
    private void InactivateDialogCam()
    {
        if (Managers.Instance.CutSceneManager.IsCutScenePlaying) return;
        dialogCam.SetActive(false);
        mainCamera.orthographicSize = Define.orthoSize;
    }

    private void OnDestroy()
    {
        cutSceneManager.OnCutSceneStart -= InactivateLight;
        cutSceneManager.OnCutSceneEnd -= ActiveLight;
    }
}