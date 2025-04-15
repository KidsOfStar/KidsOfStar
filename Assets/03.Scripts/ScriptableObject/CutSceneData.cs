using System;
using UnityEngine;

[Serializable]
public class CutSceneData
{
    [field: SerializeField] public int CutSceneID { get; private set; }
    [field:SerializeField] public int[] DialogIndexes { get; private set; }
    [field:SerializeField] public NPC[] Npcs { get; private set; }
}