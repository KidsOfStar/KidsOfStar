using UnityEngine;

public class BuildSize : MonoBehaviour
{
    private void Awake()
    {
        int height = 1080;
        int width = (int)(height * (9f / 16f));

        Screen.SetResolution(width, height, false);
    }
}
