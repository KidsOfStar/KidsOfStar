
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        continueButton.onClick.AddListener(() => Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title));
        continueButton.interactable = false; // 처음엔 비활성화
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
        clickToContinueText.gameObject.SetActive(false);
        continueButton.interactable = false;

        StartCoroutine(PlayEndingFlow());
    }

    private IEnumerator PlayEndingFlow()
    {
        yield return new WaitForSeconds(3f);

        clickToContinueText.gameObject.SetActive(true);
        StartCoroutine(BlinkText());

        continueButton.interactable = true; // 버튼 활성화
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
}
