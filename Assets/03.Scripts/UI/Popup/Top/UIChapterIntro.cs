using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIChapterIntro : UIBase
{
    [SerializeField] private TextMeshProUGUI introText;
    private readonly WaitForSeconds showTime = new(1.5f);

    public IEnumerator IntroCoroutine(bool isFirst, string text, Action completeCallback)
    {
        if (isFirst)
        {
            introText.text = text;
            yield return showTime;
        }

        HideDirect();
        completeCallback?.Invoke();
    }
}