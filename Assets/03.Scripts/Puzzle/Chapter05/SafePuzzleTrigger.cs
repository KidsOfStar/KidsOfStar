using UnityEditor;
using UnityEngine;

public class SafePuzzleTrigger : PuzzleTriggerBase
{
    [Header("상호작용 이펙트")]
    public SceneType sceneType;
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        if (triggered || Managers.Instance.GameManager.ChapterProgress != requiredProgress)
            return;

        if (collision.CompareTag("Player"))
        {
            var formControl = Managers.Instance.GameManager.Player?.FormControl.CurFormData.playerFormType;
            if (!(formControl == PlayerFormType.Human || formControl == PlayerFormType.Hide ))
            {
                Managers.Instance.UIManager.Show<WarningPopup>(WarningType.Squirrel);
                return; // 인간 폼이 아니면 상호작용 불가
            }

            SetupInteraction(); // 상호작용 버튼 설정
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Managers.Instance.GameManager.ChapterProgress == requiredProgress)
        {
            hasPlayer = true;   // 플레이어가 트리거 안에 있음
            SetupInteraction(); // 상호작용 버튼 설정
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideInteraction();
        }
    }


    protected override void TryStartPuzzle()
    {
        base.TryStartPuzzle();

        if (!isTutorial)
        {
            isTutorial = true;
            // 튜토리얼 팝업 표시
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

    protected override void OnPuzzleButtonPressed()
    {
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.Communication);
        TryStartPuzzle();
    }

}
