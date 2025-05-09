using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    [Header("페이드 효과에 사용될 이미지")]
    public Image fadePanel;
    [SerializeField]
    [Range(0.0f, 10f)] private float fadeTime;
    [SerializeField] private float durationTime;

    public IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        Color alpha = fadePanel.color;
        while (alpha.a > 0f)
        {
            fadeTime += Time.deltaTime / durationTime;
            alpha.a = Mathf.Lerp(1, 0, fadeTime);
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        Color alpha = fadePanel.color;
        while(alpha.a < 1f)
        {
            fadeTime += Time.deltaTime / durationTime;
            alpha.a = Mathf.Lerp(0, 1, fadeTime);
            fadePanel.color = alpha;
            yield return null;
        }
        yield return null;
    }
}
