using System;

public static class Extensions
{
    public static string GetName(this SoundType soundType)
    {
        return string.Empty;
    }
    
    public static string GetName(this FootstepType footstepType)
    {
        return string.Empty;
    }
    
    public static string GetName(this SceneType sceneType)
    {
        return sceneType switch
        {
            SceneType.Title     => "TitleScene",
            SceneType.Loading   => "LoadingScene",
            SceneType.Chapter01 => "Chapter_01",
            _                   => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
        };
    }
    
    public static string GetName(this ChapterType chapterType)
    {
        return chapterType switch
        {
            ChapterType.Chapter01 => "Chapter1",
            ChapterType.Chapter02 => "Chapter2",
            ChapterType.Chapter03 => "Chapter3",
            _                     => throw new ArgumentOutOfRangeException(nameof(chapterType), chapterType, null)
        };
    }
}
