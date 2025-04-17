using Unity.Mathematics;
using UnityEngine;

public class NPC : MonoBehaviour, IDialogSpeaker
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public Transform BubblePos { get; private set; }

    // 자유상호작용의 대화를 들고 있어야함
    // 인덱스로 현재 챕터의 대사만 들고 온다.
    // 프로그레스에 따라 맞는 인덱스의 대사를 출력한다.

    public CharacterType GetCharacterType()
    {
        return CharacterType;
    }

    public Vector3 GetBubblePosition()
    {
        return BubblePos.position;
    }
    
    // test
    private void Update()
    {
        if (CharacterType != CharacterType.Maorum)
            return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Managers.Instance.DialogueManager.SetCurrentDialogData(10016);
            // Managers.Instance.SaveManager.Save(0, null);

            // var test = Instantiate(cutScene, Vector3.zero, quaternion.identity);
        }
    }
}
