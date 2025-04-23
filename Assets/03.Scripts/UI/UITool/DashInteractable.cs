using UnityEngine;

public class DashInteractable : MonoBehaviour
{
    private bool playerInRange = false;
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
            playerInRange = true;
            skillBTN.ShowInteractionButton(true); // 버튼 표시
            skillBTN.OnInteractBtnClick += OnPlayerInteract;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            skillBTN.ShowInteractionButton(false); // 버튼 숨김
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
        }
    }

    private void OnPlayerInteract()
    {
        if (!playerInRange)
        {
            dashGame.StartGame(); // StartGame 호출
            skillBTN.ShowInteractionButton(true);
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
        }
        else
        {
            dashGame.EndGame(); // EndGame 호출
            skillBTN.ShowInteractionButton(false);
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
        }
    }

}
