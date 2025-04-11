using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : UIBase, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBase;
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField, Range(5f, 20f)] private float sensitivity = 5f;

    private Vector2 inputVector = Vector2.zero;
    public Vector2 Direction => inputVector;

    private Vector2 startPos;
    private float maxDistance;

    private void Start()
    {
        startPos = joystickHandle.position;
        maxDistance = joystickBase.sizeDelta.x / 2f;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        var direction = eventData.position - startPos;
        var distance = direction.magnitude;

        if (distance < sensitivity)
        {
            inputVector = Vector2.zero;
            return;
        }
        
        direction = direction.normalized;
        distance = Mathf.Min(distance, maxDistance);
        inputVector = direction;
        
        joystickHandle.position = startPos + direction * distance;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.position = startPos;
    }
}