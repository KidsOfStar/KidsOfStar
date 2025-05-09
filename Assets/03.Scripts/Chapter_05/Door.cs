using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject bubbleTextPrefab;
    private GameObject bubbleTextInstance; // 문 위에 생성된 프리팹 인스턴스

    // 이동할 씬 설정하기
    public SceneType sceneType;

    // 문 잠금 여부
    public bool isDoorLocked = false;
    private SkillBTN skillBTN;
    // 문과 닿을 시 상호작용 키 활성화

    private void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        // 상호작용 버튼 클릭 이벤트 등록
        skillBTN.OnInteractBtnClick += OnInteraction;
    }
    private void OnDestroy()
    {
        if (skillBTN != null)
        {
            skillBTN.OnInteractBtnClick -= OnInteraction; // 상호작용 버튼 클릭 이벤트 해제
        }
    }

    private void OnInteraction()
    {
        if (!isDoorLocked)
        {
            if (bubbleTextInstance == null && bubbleTextPrefab != null)
            {
                Debug.Log("문이 잠겨 있음 - 버블 생성");
                bubbleTextInstance = Instantiate(bubbleTextPrefab, transform);
                bubbleTextInstance.transform.localPosition = new Vector3(0, 2f, 0); // 문 위 위치

                var bubbleText = bubbleTextInstance.GetComponentInChildren<DoorPopup>();
                if (bubbleText != null)
                {
                    bubbleText.Opened(); // 문구 출력
                }
            }
            else if (bubbleTextInstance != null)
            {
                Debug.Log("이미 생성됨 - 제거");
                Destroy(bubbleTextInstance);
                bubbleTextInstance = null;
            }
        }
        else
        {
            Managers.Instance.SceneLoadManager.LoadScene(sceneType);
        }
    }

    // 문과 닿을 시 상호작용 키 활성화
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(true); // 상호작용 버튼 활성화
            
        }
    }

    // 문과 떨어질 시 상호작용 키 비활성화
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            skillBTN.ShowInteractionButton(false); // 상호작용 버튼 비활성화

            if (bubbleTextInstance != null)
            {
                Destroy(bubbleTextInstance);
                bubbleTextInstance = null;
            }
            skillBTN.OnInteractBtnClick -= OnInteraction; // 상호작용 버튼 클릭 이벤트 해제
        }
    }
}

