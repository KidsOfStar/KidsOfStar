using UnityEngine;

public class LetterBox : MonoBehaviour
{
    private float fixedAspectRatio = 16.0f / 9.0f;

    private void Start()
    {
        EditorLog.Log("LetterBox: Start() 호출됨.");
        Camera cam = GetComponent<Camera>();
        if (cam == null)
        {
            EditorLog.LogError("LetterBox: Camera 컴포넌트를 찾을 수 없습니다!");
            return;
        }
        AdjustCamView(cam);
    }
    private void AdjustCamView(Camera cam)
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        if (Mathf.Approximately(currentAspectRatio, fixedAspectRatio))
        {
            cam.rect = new Rect(0f, 0f, 1f, 1f);
            EditorLog.Log("LetterBox: 종횡비가 거의 동일하여 전체 화면으로 설정합니다.");
            return;
        }
        else if (currentAspectRatio > fixedAspectRatio)
        {

            float w = fixedAspectRatio / currentAspectRatio;
            float x = (1f - w) / 2f;

            cam.rect = new Rect(x, 0f, w, 1f);
            EditorLog.Log($"LetterBox: 화면이 넓어서 좌우 여백 생성. 계산된 rect: {cam.rect}");
        }
        else
        {
            float h = currentAspectRatio / fixedAspectRatio;
            float y = (1f - h) / 2f;

            cam.rect = new Rect(0f, y, 1f, h);
            EditorLog.Log($"LetterBox: 화면이 높아서 상하 여백 생성. 계산된 rect: {cam.rect}");
        }
    }
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
        EditorLog.Log("LetterBox: OnPreCull() 호출됨 - 화면을 검은색으로 클리어합니다.");
    }

}
