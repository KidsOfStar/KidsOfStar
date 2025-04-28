using UnityEngine;
public class SceneNpc : InteractSpeaker, IDialogSpeaker
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
