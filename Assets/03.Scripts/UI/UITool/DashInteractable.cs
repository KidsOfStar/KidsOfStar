using System.Collections.Generic;
using UnityEngine;

public class DashInteractable : MonoBehaviour
{
    public InteractionType interactionType; // 상호작용 타입
    public CharacterType npcType; // Jigim 또는 Semyung을 에디터에서 지정
    //public int dialogIndex; // 대사 인덱스

    private SkillBTN skillBTN;
    private DashGame dashGame;

    // Start is called before the first frame update
    public void Init()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel; // 스킬 버튼 UI
        dashGame = FindObjectOfType<DashGame>();


        // 대사 완료 이벤트 등록
        Managers.Instance.DialogueManager.OnDialogStepEnd += CheckDialogueCompletion;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        Managers.Instance.DialogueManager.OnDialogStepEnd -= CheckDialogueCompletion;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Managers.Instance.GameManager.ChapterProgress == 2)
        {
            skillBTN.ShowInteractionButton(true); // 버튼 표시
            skillBTN.OnInteractBtnClick += OnPlayerInteract;
        }
        else skillBTN.OnInteractBtnClick -= OnPlayerInteract;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(false); // 버튼 숨김
            skillBTN.OnInteractBtnClick -= OnPlayerInteract;
        }
        else return;

    }

    private void OnPlayerInteract()
    {
        if (interactionType == InteractionType.EndGame)
        {
            dashGame.EndGame(npcType); // NPC 정보를 함께 전달
        }
    }

    private void CheckDialogueCompletion(int completedDialogIndex)
    {
        if (completedDialogIndex == 30006)
        {
            PlayerTeleport(); // 플레이어 위치 변경
        }
        if (completedDialogIndex == 30007)
        {
            Debug.Log("30007번 대사가 완료되었습니다.");
            dashGame.StartGame();
        }
    }

    private void PlayerTeleport()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(-7f, 1.4f, 0); // 예시 위치
            Debug.Log("플레이어 위치 변경됨: " + player.transform.position);
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }
}
