using UnityEngine;

public class TreePuzzleTrigger : MonoBehaviour
{
    private bool triggered = false;
    private bool hasPlayer = false;
    private bool hasBox = false;

    private SkillBTN skillBTN;
    [SerializeField] private TreePuzzleData puzzleData;
    [SerializeField] private int sequenceIndex;
    public int SequenceIndex => sequenceIndex;

    [SerializeField] private GameObject exclamationInstance;
    private SpriteRenderer exclamationRenderer;

    [Header("트리거가 켜질 ChapterProgress 값")]
    [SerializeField] private int requiredProgress;

    private void Awake()
    {
        // SpriteRenderer 컴포넌트 가져오기
        exclamationRenderer = exclamationInstance.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        bool show = Managers.Instance.GameManager.ChapterProgress == requiredProgress;
        exclamationRenderer.enabled = show;

        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

       Managers.Instance.GameManager.OnProgressUpdated += UpdateExclamation;
    }

    // 게임 클리어시 비활성화
    public void DisableExclamation()
    {
        exclamationRenderer.enabled = false;
    }

    private void UpdateExclamation()
    {
        bool show = Managers.Instance.GameManager.ChapterProgress == requiredProgress;
        exclamationRenderer.enabled = show;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || Managers.Instance.GameManager.ChapterProgress != requiredProgress) return;

        if (collision.CompareTag("Player"))
        {
            hasPlayer = true;

            // 다람쥐 형태 검사
            var formControl = Managers.Instance.GameManager.Player.FormControl;
            bool isSquirrel = formControl.ReturnCurFormName() == "Squirrel";

            if (isSquirrel && !hasBox)
            {
                // 다람쥐 & 상자 없음 → 두 문구를 순차적으로
                Managers.Instance.UIManager.Show<TreeWarningPopup>(
                    WarningType.Squirrel,
                    WarningType.BoxMissing
                );
                return;
            }
            else if (isSquirrel)
            {
                // 다람쥐지만 상자는 들고 있는 경우 → 한 문구만
                Managers.Instance.UIManager.Show<TreeWarningPopup>(
                    WarningType.Squirrel
                );
                return;
            }

            if (!hasBox)
            {
                // 다람쥐가 아니지만, 상자가 없는 경우
                Managers.Instance.UIManager.Show<TreeWarningPopup>(
                    WarningType.BoxMissing
                );
            }
        }
        else if (collision.CompareTag("Box"))
        {
            hasBox = true;
        }
        else
        {
            return;
        }

        TryEnableInteraction();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            hasPlayer = false;
        }
        else if (collision.CompareTag("Box"))
        {
            hasBox = false;
        }
        else
        {
            return;
        }

        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= TryStartPuzzle;
    }

    private void TryEnableInteraction()
    {
        if (hasPlayer && hasBox)
        {
            skillBTN.ShowInteractionButton(true);
            skillBTN.OnInteractBtnClick += () =>
            {
                Managers.Instance.SoundManager.PlaySfx(SfxSoundType.Communication);
                TryStartPuzzle();
            };
        }
    }

    private void TryStartPuzzle()
    {
        if (triggered) return;

        triggered = true;

        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= TryStartPuzzle;

        Managers.Instance.UIManager.Show<TreePuzzlePopup>(puzzleData, sequenceIndex);
    }

    public void ResetTrigger()
    {
        triggered = false;

        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= TryStartPuzzle;

        if (hasPlayer && hasBox)
            TryEnableInteraction();
    }

    private void OnDestroy()
    {
        Managers.Instance.GameManager.OnProgressUpdated -= UpdateExclamation;
    }
}
