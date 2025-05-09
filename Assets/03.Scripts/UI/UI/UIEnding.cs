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

    [Header("Fade Components")]
    public FadeEffect backgroundFadeEffect;

    [Header("엔딩 일러스트 목록")]
    [SerializeField] private List<SpritePair> endingSpriteList;

    private Dictionary<EndingType, Sprite> endingSpriteDict;
    private bool canClick = false;

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

        backgroundFadeEffect.fadePanel.color = new Color(0, 0, 0, 1f);
        clickToContinueText.gameObject.SetActive(false);
        endingImage.gameObject.SetActive(false);
    }
    
    public override void Opened(params object[] param)
    {
        base.Opened(param);

        if (param.Length == 0) return;

        if (!(param[0] is EndingType)) return;

        EndingType endingType = (EndingType)param[0];

        if (!endingSpriteDict.TryGetValue(endingType, out var sprite))
        {
            return;
        }

        endingImage.sprite = sprite;
        StartCoroutine(PlayEndingFlow());
    }

    private IEnumerator PlayEndingFlow()
    {
        yield return StartCoroutine(backgroundFadeEffect.FadeIn());
        yield return new WaitForSeconds(1f);

        endingImage.gameObject.SetActive(true);

        canClick = true;
        clickToContinueText.gameObject.SetActive(true);
        continueButton.interactable = true; // 버튼 활성화
        StartCoroutine(BlinkText());
    }

    private IEnumerator OnContinue()
    {
        canClick = false;
        clickToContinueText.gameObject.SetActive(false);
        continueButton.interactable = false;

        yield return StartCoroutine(backgroundFadeEffect.FadeOut());

        yield return new WaitForSeconds(1f);
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
}
