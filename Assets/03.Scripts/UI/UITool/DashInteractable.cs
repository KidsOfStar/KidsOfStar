using UnityEngine;

public enum EInteractionType
{
    StartGame,
    EndGame
}

public class DashInteractable : MonoBehaviour
{
    public EInteractionType interactionType; // 상호작용 타입

    private bool oneTime = false; // 한번만 작동하게 하기
    public SkillBTN skillBTN;
    private DashGame dashGame;

    // Start is called before the first frame update
    void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        dashGame = FindObjectOfType<DashGame>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oneTime = true;
            skillBTN.ShowInteractionButton(true); // 버튼 표시
            skillBTN.OnInteractBtnClick += OnPlayerInteract;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oneTime = false;
            skillBTN.ShowInteractionButton(false); // 버튼 숨김
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
        }
    }

    private void OnPlayerInteract()
    {

        if ((interactionType == EInteractionType.StartGame) && )
        {
            dashGame.StartGame();
        }
        else if (interactionType == EInteractionType.EndGame)
        {
            dashGame.EndGame();
        }

        // 한 번만 작동하게 하고 싶다면 버튼 비활성화
        skillBTN.ShowInteractionButton(false);
        skillBTN.OnInteractBtnClick -= OnPlayerInteract;
    }

}
