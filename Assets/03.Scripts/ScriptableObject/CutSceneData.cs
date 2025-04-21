using System;
using UnityEngine;

[Serializable]
public class CutSceneData
{
    [field: SerializeField] public int[] DialogIndexes { get; private set; }
    [field: SerializeField] public Npc[] Npcs { get; private set; }
}