using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : UIBase, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBase;
    [SerializeField] private RectTransform joystickHandle;

    private Vector2 inputVector = Vector2.zero;
    public Vector2 Direction => inputVector;

    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        EditorLog.Log(Direction);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // 처음 눌렀을 때도 바로 처리
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                                                joystickBase, eventData.position,
                                                                mainCamera, out Vector2 pos);

        // 베이스 반지름 기준 정규화
        float radius = joystickBase.sizeDelta.x / 2f;
        inputVector = pos / radius;

        inputVector = inputVector.magnitude > 1f ? inputVector.normalized : inputVector;

        // 핸들 위치 설정
        joystickHandle.anchoredPosition = inputVector * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}