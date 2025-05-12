using Febucci.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterIntro : UIBase
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private TypewriterByCharacter typewriter;
    private const float fadeTime = 2f;
    private const float fadeOutTime = 2f;
    private readonly Color fadeOutColor = new(0, 0, 0, 0f);

    public IEnumerator IntroCoroutine(bool isFirst, string text)
    {
        if (!isFirst)
        {
            HideDirect();
            yield break;
        }

        Managers.Instance.GameManager.Player.Controller.IsControllable = false;
        introText.text = string.Empty;
        
        // 배경 페이드 인
        // yield return Fade(fadeOutColor, Color.black, fadeTime, c => backgroundImage.color = c);

        // 텍스트 페이드 인
        // yield return Fade(fadeOutColor, Color.white, fadeTime, c => introText.color = c);

        typewriter.onTextShowed.AddListener(() => StartCoroutine(CompleteTextShowed()));
        typewriter.ShowText(text);
    }

    private IEnumerator CompleteTextShowed()
    {
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(Fade(Color.white, fadeOutColor, fadeOutTime, c => introText.color = c));
        yield return Fade(Color.black, fadeOutColor, fadeOutTime, c => backgroundImage.color = c);
        
        HideDirect();
        Managers.Instance.GameManager.Player.Controller.IsControllable = true;
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

    private void OnDisable()
    {
        typewriter.onTextShowed.RemoveAllListeners();
    }
}