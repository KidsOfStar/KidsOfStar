using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class NPC : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public Transform BubblePos { get; private set; }

}
