using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequiredIndexData", menuName = "ScriptableObject/RequiredIndexData")]
public class RequiredIndexData : ScriptableObject
{
    public Dictionary<ChapterType, RequiredIndex[]> requiredIndexDict = new();
    [Header("Chapter 1")]
    public RequiredIndex[] chapter1List;

    [Header("Chapter 2")]
    public RequiredIndex[] chapter2List;
    
    [Header("Chapter 3")]
    public RequiredIndex[] chapter3List;

    public void Init()
    {
        requiredIndexDict.Clear();
        requiredIndexDict.Add(ChapterType.Chapter01, chapter1List);
        requiredIndexDict.Add(ChapterType.Chapter02, chapter2List);
        requiredIndexDict.Add(ChapterType.Chapter03, chapter3List);
    }
}

[Serializable]
public class RequiredIndex
{
    public CharacterType characterType;
    public int progress;
    public int index;
}