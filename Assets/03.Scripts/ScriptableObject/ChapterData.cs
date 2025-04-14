using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "ScriptableObject/ChapterData")]
public class ChapterData : ScriptableObject
{
    public ChapterProgress[] ChapterProgresses;
}

[Serializable]
public class ChapterProgress
{
    public ChapterType ChapterType;
    public int[] Progress;
}