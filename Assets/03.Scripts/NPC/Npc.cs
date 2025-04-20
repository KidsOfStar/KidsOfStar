using UnityEngine;

public class Npc : InteractSpeaker, IDialogSpeaker
{
    [field: SerializeField] public Transform BubbleTr { get; private set; }

    public CharacterType GetCharacterType()
    {
        return CharacterType;
    }

    public Vector3 GetBubblePosition()
    {
        return BubbleTr.position;
    }
}
