using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("상호작용 이펙트")]
    public SceneType sceneType;
    [SerializeField] private GameObject exclamationInstance;
    //private SpriteRenderer exclamationRenderer;

    [SerializeField] private GameObject bubbleTextPrefab;
    private GameObject bubbleTextInstance; // 문 위에 생성된 프리팹 인스턴스

    private SkillBTN skillBTN;
    [Header("튜토리얼 문인지 체크")]
    [SerializeField] private bool isTutorial = false;

    // 동물 폼
    [SerializeField] private PlayerFormType dangerFormMask;

    [Header("금고 번호")]
    public int safeNumber; // 각 씬의 금고 번호

    private void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(true); // 상호작용 버튼 활성화
            skillBTN.OnInteractBtnClick += OnInteraction; // 상호작용 버튼 클릭 이벤트 등록
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideInteraction();
        }
    }

    private void OnInteraction()
    {
        var player = Managers.Instance.GameManager.Player;
        var currentForm = player?.FormControl?.CurFormData;

        if(currentForm == null) return;

        if ((dangerFormMask & currentForm.playerFormType) != 0)
        {
            //OntextBubbleText(player);
            Managers.Instance.UIManager.Show<WarningPopup>(
                WarningType.Squirrel
                );
        }
        else
            TryStartPuzzle();
    }

    private void TryStartPuzzle()
    {
        if (!isTutorial)
        {
            isTutorial = true;
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(4);
            popup.OnClosed += () =>
            {
                var safePopup = Managers.Instance.UIManager.Show<SafePopup>();
                safePopup.Opened(sceneType);  // 안전한 폼일 경우 팝업 표시
                safePopup.safePuzzle.SetSafeNumber(safeNumber);
            };
            return;
        }
        // 튜토리얼 보여주고 시작
        Managers.Instance.UIManager.Show<SafePopup>().Opened(sceneType); // 안전한 폼일 경우 팝업 표시
    }

    private void OntextBubbleText(Player player)
    {
        if (bubbleTextInstance == null && bubbleTextPrefab != null)
        {
            bubbleTextInstance = Instantiate(bubbleTextPrefab, player.transform);
            bubbleTextInstance.transform.localPosition = new Vector3(0, 2f, 0);

            var bubbleText = bubbleTextInstance.GetComponentInChildren<DoorPopup>();

            if (bubbleText != null)
            {
                bubbleText.SetText("세심하게 만져야 한다. 다른 방법이 없을까?");
            }
        }
        else if (bubbleTextInstance != null)
        {
            Destroy(bubbleTextInstance);
            bubbleTextInstance = null;
        }
    }

    private void HideInteraction()
    {
        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= OnInteraction;
    }

    // 게임 클리어시 비활성화
    public void DisableExclamation()
    {
        exclamationInstance.SetActive(false);
    }
}
