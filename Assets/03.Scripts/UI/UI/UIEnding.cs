
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
    [SerializeField] private Image backgroundPanel;

    [Header("엔딩 일러스트 목록")]
    [SerializeField] private List<SpritePair> endingSpriteList;

    private Dictionary<EndingType, Sprite> endingSpriteDict;
    private bool canClick = false;

    private readonly Color transparentWhite = new Color(1f, 1f, 1f, 0f);
    private readonly Color opaqueWhite = new Color(1f, 1f, 1f, 1f);
    private readonly Color transparentBlack = new Color(0f, 0f, 0f, 0f);
    private readonly Color opaqueBlack = new Color(0f, 0f, 0f, 1f);

    private const float fadeTime = 2f;
    private const float showDelay = 3f;


    private void OnEnable()
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

        backgroundPanel.enabled = true;
        backgroundPanel.color = transparentBlack;

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
        yield return new WaitForSeconds(showDelay);


        canClick = true;
        clickToContinueText.gameObject.SetActive(true);
        continueButton.interactable = true; // 버튼 활성화
        StartCoroutine(BlinkText());
    }

    private IEnumerator OnContinue()
    {
        canClick = false;
        clickToContinueText.gameObject.SetActive(false);

        // 배경 패널 페이드 인 (투명Black → 불투명Black)
        StartCoroutine(Fade(
            from: transparentBlack,
            to: opaqueBlack,
            duration: fadeTime,
            applyColor: c => backgroundPanel.color = c
        ));

        // 엔딩 이미지 페이드 아웃 (불투명White → 투명White)
        yield return Fade(
            from: opaqueWhite,
            to: transparentWhite,
            duration: fadeTime,
            applyColor: c => endingImage.color = c
        );

        // 씬 전환
        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title);
    }

    private IEnumerator BlinkText()
    {
        while (canClick)
        {
            clickToContinueText.alpha = 1f;
            yield return new WaitForSeconds(0.5f);
            clickToContinueText.alpha = 0f;
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
