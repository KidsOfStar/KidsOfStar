using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackGround : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float xValue;

    private void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(xValue, 0) * Time.deltaTime, image.uvRect.size);
    }
}
