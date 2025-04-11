using UnityEngine;

public class NPC : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [SerializeField] private Vector3 bubbleOffset;

    public Vector3 BubbleOffset => transform.position + bubbleOffset;

    // test
    private void Update()
    {
        if (CharacterType != CharacterType.Maorum)
            return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Instance.DialogueManager.SetCurrentDialogData(1001);
        }
    }
}
