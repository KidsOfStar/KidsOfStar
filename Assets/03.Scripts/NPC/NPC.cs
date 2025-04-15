using Unity.Mathematics;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public Transform BubblePos { get; private set; }

    [SerializeField] public GameObject cutScene;
    
    // test
    private void Update()
    {
        if (CharacterType != CharacterType.Maorum)
            return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Managers.Instance.DialogueManager.SetCurrentDialogData(10016);
            // Managers.Instance.SaveManager.Save(0, null);

            Instantiate(cutScene, Vector3.zero, quaternion.identity);
        }
    }
}
