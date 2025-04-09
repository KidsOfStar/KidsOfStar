using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Vector3 speechBubbleOffset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // setText
        }
    }
}
