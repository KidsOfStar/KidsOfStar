using UnityEngine;
public class Npc : InteractSpeaker, IDialogSpeaker
{
    public CharacterType GetCharacterType()
    {
        return CharacterType;
    }

    public Vector3 GetBubblePosition()
    {
        return BubbleTr.position;
    }
}
