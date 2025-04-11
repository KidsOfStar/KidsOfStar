using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float boxWeight = 2f; // 박스 무게 설정 (예시값)

    [HideInInspector] public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1f;
        rb.mass = boxWeight;
    }

    public float GetWeight()
    {
        return boxWeight;
    }
}