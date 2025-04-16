using UnityEngine;

public class BuildSize : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_WEBGL
        int height = 1080;
        int width = (int)(height * (9f / 16f));

        Screen.SetResolution(width, height, false);

#elif UNITY_ANDROID
        int height = 1080;
        int width = (int)(height * (9f / 16f));

        Screen.SetResolution(width, height, false);
#endif
    }
}
