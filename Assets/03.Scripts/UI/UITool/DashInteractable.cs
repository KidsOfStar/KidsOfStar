using UnityEngine;


public class DashInteractable : MonoBehaviour
{
    public InteractionType interactionType; // 상호작용 타입

    public NPCType npcType; // Jigim 또는 Semyung을 에디터에서 지정

    SkillBTN skillBTN;
    Npc npc;
    private DashGame dashGame;

    // Start is called before the first frame update
    void Start()
    {
        dashGame = FindObjectOfType<DashGame>();
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel; // 스킬 버튼 UI
        npc = GetComponent<Npc>(); // Npc 컴포넌트 가져오기
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Managers.Instance.GameManager.ChapterProgress == 2)
        {
            skillBTN.ShowInteractionButton(true); // 버튼 표시
            skillBTN.OnInteractBtnClick += OnPlayerInteract;
            npc.enabled = false; // NPC 활성화
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Managers.Instance.GameManager.ChapterProgress == 2)
        {
            skillBTN.ShowInteractionButton(false); // 버튼 숨김
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
            npc.enabled = true; // NPC 활성화
        }
    }

    private void OnPlayerInteract()
    {
        if (interactionType == InteractionType.StartGame && !dashGame.isGameStarted)
        {
            dashGame.StartGame();
            this.enabled = false; // 스크립트 비활성화
            skillBTN.ShowInteractionButton(false);
        }
        else if (interactionType == InteractionType.EndGame)
        {
            dashGame.EndGame(npcType); // NPC 정보를 함께 전달
        }
    }
}