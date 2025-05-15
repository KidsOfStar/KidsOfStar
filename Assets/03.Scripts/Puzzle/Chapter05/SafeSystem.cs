using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSystem : MonoBehaviour
{
    [SerializeField] private GameObject bubbleTextPrefab;
    private GameObject bubbleTextInstance; // 문 위에 생성된 프리팹 인스턴스

    private SkillBTN skillBTN;

    // 동물 폼
    [SerializeField] private PlayerFormType dangerFormMask;

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
            skillBTN.ShowInteractionButton(false); // 상호작용 버튼 비활성화
            skillBTN.OnInteractBtnClick -= OnInteraction; // 상호작용 버튼 클릭 이벤트 해제
        }
    }

    private void OnInteraction()
    {
        var player = Managers.Instance.GameManager.Player;
        var currentForm = player?.FormControl?.CurFormData;

        if(currentForm == null) return;

        if ((dangerFormMask & currentForm.playerFormType) != 0)
        {
            OntextBubbleText(player);
        }
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
}
