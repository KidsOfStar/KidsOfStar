using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [Header("Loading Data")]
    [SerializeField] private LoadingData loadingData;

    [Header("Loading UI")]
    [SerializeField] private Image backgroundImage;

    [SerializeField] private TextMeshProUGUI tooltipText;

    private readonly WaitForSeconds tooltipWaitTime = new(2f);
    private int currentTooltipIndex = -1;

    private void Start()
    {
        if (loadingData == null)
        {
            EditorLog.LogError("LoadingData is not assigned in the inspector.");
            return;
        }

        SetRandomBackground();
        StartCoroutine(TooltipCoroutine());
    }

    private IEnumerator TooltipCoroutine()
    {
        if (loadingData.Tooltips.Length == 0)
        {
            EditorLog.LogError("No tooltips found in LoadingData.");
            yield break;
        }

        while (true)
        {
            int randomIndex;
            do
                randomIndex = Random.Range(0, loadingData.Tooltips.Length);
            while (randomIndex == currentTooltipIndex);
            
            currentTooltipIndex = randomIndex;
            tooltipText.text = loadingData.Tooltips[randomIndex];
            yield return tooltipWaitTime;
        }
    }

    private void SetRandomBackground()
    {
        if (loadingData.Backgrounds.Length == 0)
        {
            EditorLog.LogError("No background images found in LoadingData.");
            return;
        }

        int randomIndex = Random.Range(0, loadingData.Backgrounds.Length);
        backgroundImage.sprite = loadingData.Backgrounds[randomIndex];
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}