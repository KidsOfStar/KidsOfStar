using UnityEngine;

public class NPC : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [SerializeField] private Vector3 bubbleOffset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Instance.DialogueManager.SetCurrentDialogData(2000, bubbleOffset);
        }
    }
}
