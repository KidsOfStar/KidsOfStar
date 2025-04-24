
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnding : UIBase
{
    [System.Serializable]
    public struct SpritePair
    {
        public EndingType type;
        public Sprite sprite;
    }

    [Header("UI Components")]
    [SerializeField] private Image endingImage;
    [SerializeField] private TextMeshProUGUI clickToContinueText;
    [SerializeField] private Button continueButton;

    [Header("엔딩 일러스트 목록")]
    [SerializeField] private List<SpritePair> endingSpriteList;

    private Dictionary<EndingType, Sprite> endingSpriteDict;
    private bool canClick = false;

    private readonly Color transparentWhite = new Color(1f, 1f, 1f, 0f);
    private readonly Color opaqueWhite = new Color(1f, 1f, 1f, 1f);
    private const float fadeTime = 2f;
    private const float showDelay = 3f;


    private void Awake()
    {
        // Dictionary 초기화
        endingSpriteDict = new Dictionary<EndingType, Sprite>();
        foreach (var pair in endingSpriteList)
        {
            if (!endingSpriteDict.ContainsKey(pair.type))
            {
                endingSpriteDict.Add(pair.type, pair.sprite);
            }
        }
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => StartCoroutine(OnContinue()));
        continueButton.interactable = false; // 처음엔 비활성화

        endingImage.color = transparentWhite;
        clickToContinueText.gameObject.SetActive(false);
    }

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        if (param.Length == 0) return;

        if (!(param[0] is EndingType))return;

        EndingType endingType = (EndingType)param[0];

        if (!endingSpriteDict.TryGetValue(endingType, out var sprite))
        {
            EditorLog.LogWarning($"[UIEnding] {endingType}에 해당하는 이미지가 없습니다!");
            return;
        }

        endingImage.sprite = sprite;
        StartCoroutine(PlayEndingFlow());
    }

    private IEnumerator PlayEndingFlow()
    {
        yield return Fade(
         from: transparentWhite,
         to: opaqueWhite,
         duration: fadeTime,
         applyColor: c => endingImage.color = c
     );
        canClick = true;
        clickToContinueText.gameObject.SetActive(true);
        StartCoroutine(BlinkText());
        continueButton.interactable = true; // 버튼 활성화
    }

    private IEnumerator OnContinue()
    {
        yield return Fade(
        from: opaqueWhite,
        to: transparentWhite,
        duration: fadeTime,
        applyColor: c => endingImage.color = c
    );


        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title);
    }
    private IEnumerator BlinkText()
    {
        while (canClick)
        {
            clickToContinueText.alpha = 1;
            yield return new WaitForSeconds(0.5f);
            clickToContinueText.alpha = 0;
            yield return new WaitForSeconds(0.5f);
        }
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
