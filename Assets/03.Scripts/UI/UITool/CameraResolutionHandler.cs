using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResolutionHandler : MonoBehaviour
{
    [SerializeField] float targetWidth = 1920f;
    [SerializeField] float targetHeight = 1080f;

    // Start is called before the first frame update
    void Start()
    {
        AdjustCameraSize();
    }

    private void AdjustCameraSize()
    {
        Camera cam = GetComponent<Camera>();

        float targetAspect = targetWidth / targetHeight;    //16 : 9
        float currentAspect = (float)Screen.width / (float)Screen.height; // 현재 화면 비율

        float size = cam.orthographicSize; // 카메라의 orthographicSize를 가져옴

        if(currentAspect < targetAspect) // 현재 화면 비율이 목표 화면 비율보다 작으면
        {
            size *= targetAspect / currentAspect; // 카메라의 orthographicSize를 조정
        }
        else
        {
            size /= targetAspect / currentAspect; // 카메라의 orthographicSize를 조정
        }

    }


}
