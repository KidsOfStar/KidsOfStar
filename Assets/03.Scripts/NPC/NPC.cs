using UnityEngine;

public class NPC : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public Transform BubblePos { get; private set; }

    // test
    private void Update()
    {
        if (CharacterType != CharacterType.Maorum)
            return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Instance.DialogueManager.SetCurrentDialogData(10016);
        }
    }
}
