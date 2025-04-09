using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Vector3 bubbleOffset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Instance.DialogueManager.SetCurrentDialogData(2000, bubbleOffset);
        }
    }
}
