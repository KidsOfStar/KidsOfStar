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

    private void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            hasPlayer = true;

            // 다람쥐 형태 검사
            var formControl = Managers.Instance.GameManager.Player.FormControl;
            // 허용되지 않은 형태일 경우
            if (formControl.ReturnCurFormName() == "Squirrel")
            {
                Managers.Instance.UIManager.Show<TreeWarningPopup>();
                return;
            }
        }

        else if(collision.CompareTag("Box"))
        {
            hasBox = false;
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
            skillBTN.OnInteractBtnClick += TryStartPuzzle;
        }
    }

    private void TryStartPuzzle()
    {
        if (triggered) return;

        triggered = true;

        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= TryStartPuzzle;

        Managers.Instance.UIManager.Show<TreePuzzlePopup>(puzzleData,sequenceIndex);
    }

    public void ResetTrigger()
    {
        triggered = false;
        hasPlayer = false;
        hasBox = false;

        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= TryStartPuzzle;
    }

}
