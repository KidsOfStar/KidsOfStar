using UnityEngine;

public class CutSceneData : MonoBehaviour
{
    [field: SerializeField] public int CutSceneID { get; private set; }
    [field:SerializeField] public int[] DialogIndexes { get; private set; }
    [field:SerializeField] public NPC[] Npcs { get; private set; }
}
