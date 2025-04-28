using UnityEngine;

public class CutSceneNpc : MonoBehaviour, IDialogSpeaker
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Transform bubblePosition;
    
    public CharacterType GetCharacterType()
    {
        return characterType;
    }
    
    public Vector3 GetBubblePosition()
    {
        return bubblePosition.position;
    }
}
