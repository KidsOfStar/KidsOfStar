using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RequiredIndexData", menuName = "ScriptableObject/RequiredIndexData")]
public class RequiredIndexData : ScriptableObject
{
    public RequiredIndex[] requiredIndexList;
}

[Serializable]
public class RequiredIndex
{
    public CharacterType characterType;
    public int progress;
    public int index;
}