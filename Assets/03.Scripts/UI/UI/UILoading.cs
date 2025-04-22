using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [Header("Loading Data")]
    [SerializeField] private LoadingData androidData;

    [SerializeField] private LoadingData webGLData;
    private LoadingData currentLoadingData;

    [Header("Loading UI")]
    [SerializeField] private Image backgroundImage;

    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private string[] tooltips;

    private readonly WaitForSeconds tooltipWaitTime = new(2f);
    private int currentTooltipIndex;

    private void Start()
    {
        if (androidData == null || webGLData == null)
        {
            EditorLog.LogError("LoadingData is not assigned in the inspector.");
            return;
        }

#if UNITY_EDITOR
        currentLoadingData = androidData;
#elif UNITY_ANDROID
        currentLoadingData = androidData;
#elif UNITY_WEBGL
        currentLoadingData = webGLData;
#endif

        SetRandomBackground();
        StartCoroutine(TooltipCoroutine());
    }

    private IEnumerator TooltipCoroutine()
    {
        if (currentLoadingData.Tooltips.Length == 0)
        {
            EditorLog.LogError("No tooltips found in LoadingData.");
            yield break;
        }

        for (int i = 0; i < currentLoadingData.Tooltips.Length; i++)
        {
            var tooltipData = currentLoadingData.Tooltips[i];
            var nextScene = Managers.Instance.SceneLoadManager.NextSceneToLoad;
            if (tooltipData.sceneType != nextScene) continue;

            tooltips = tooltipData.tooltips;
            break;
        }

        if (tooltips == null || tooltips.Length == 0)
        {
            EditorLog.LogError("No tooltips found for the current scene.");
            yield break;
        }

        while (true)
        {
            if (currentTooltipIndex >= tooltips.Length)
            {
                currentTooltipIndex = 0;
            }

            tooltipText.text = tooltips[currentTooltipIndex];
            currentTooltipIndex++;
            yield return tooltipWaitTime;
        }
    }

    private void SetRandomBackground()
    {
        if (currentLoadingData.Backgrounds.Length == 0)
        {
            EditorLog.LogError("No background images found in LoadingData.");
            return;
        }

        int randomIndex = Random.Range(0, currentLoadingData.Backgrounds.Length);
        backgroundImage.sprite = currentLoadingData.Backgrounds[randomIndex];
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}