using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBoxer : MonoBehaviour
{    
    //화면비율의 기준을 선택하되, X:Y비율 / widht:height 해상도 기준으로 선택가능
    

    public Color matteColor = new Color(0, 0, 0, 1);
    public ReferenceMode referenceMode; 
    public float x=16;
    public float y=9;  
    public float width = 1920;
    public float height = 1080;
    public bool onAwake = true;
    public bool onUpdate = true;

    private Camera cam;
    private Camera letterBoxerCamera;

    public void Awake()
    {
        // store reference to the camera
        cam = GetComponent<Camera>();

        // add the letterboxing camera
        AddLetterBoxingCamera();

        // Awake일때 자동으로 크기를 맞출지 여부를 확인
        if (onAwake)
        {
            PerformSizing();
        }
    }

    public void Update()
    {
        // Update일때 자동으로 크기를 맞출지 여부를 확인
        if (onUpdate)
        {
            PerformSizing();
        }
    }

    private void OnValidate()
    {
        x = Mathf.Max(1, x);
        y = Mathf.Max(1, y);
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
    }

    private void AddLetterBoxingCamera()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera camera in allCameras)
        {             
            if (camera.depth == -100)
            {
                EditorLog.LogError("Found " + camera.name + " with a depth of -100. Will cause letter boxing issues. Please increase it's depth.");
            }
        }

        // 빈 Obj에 Camera 컴포넌트를 추가하여  검정 여백을 그려줄 로직
        letterBoxerCamera = new GameObject().AddComponent<Camera>();
        letterBoxerCamera.backgroundColor = matteColor;
        //아무것도 렌더링하지 않음.
        letterBoxerCamera.cullingMask = 0;
        //가장 먼저 그려서 가장 뒤에 배경이 되도록
        letterBoxerCamera.depth = -100;
        // 최적화를 하기위한 설정
        letterBoxerCamera.farClipPlane = 1;
        letterBoxerCamera.useOcclusionCulling = false;
        letterBoxerCamera.allowHDR = false;
        letterBoxerCamera.allowMSAA = false;
        //선택한 색으로 배경을 칠하는 로직
        letterBoxerCamera.clearFlags = CameraClearFlags.Color;       
    }

    public void PerformSizing()
    {
        float targetRatio = x / y;

        if (referenceMode == ReferenceMode.OrginalResolution)
        {
            targetRatio = width / height;
        }

        float windowaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = windowaspect / targetRatio;

        if (scaleheight < 1.0f) // 레터박스 화면비가 기준보다 더 넓은 경우
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            cam.rect = rect;
        }
        else // 필러박스 상하로 넓은 경우
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
    }
}
