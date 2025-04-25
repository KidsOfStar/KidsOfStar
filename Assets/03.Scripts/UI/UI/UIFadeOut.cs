using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeOut : UIBase
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;

    private readonly WaitForSeconds waitForFade = new(0.5f);
    private readonly Color fadeOutColor = new Color(0, 0, 0, 0f);

    public void StartFadeOut(Action onComplete)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        yield return waitForFade;
        
        // 블랙 -> 투명
        yield return Fade(Color.black, fadeOutColor, fadeDuration, c => fadeImage.color = c);
        onComplete?.Invoke();
        HideDirect();
    }

    private IEnumerator Fade(Color from, Color to, float duration, Action<Color> applyColor)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            applyColor(Color.Lerp(from, to, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        applyColor(to);
    }
}