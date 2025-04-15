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
    
    public static string GetName(this EndingType endingType)
    {
        return endingType switch
        {
            EndingType.ComfortableLife  => "안락한 일상",
            EndingType.WinRecognition   => "쟁취한 인정",
            EndingType.DreamingCat      => "낭만 고양이",
            EndingType.IntroTheOcean    => "드넓은 바다로",
            EndingType.DifferentButSame => "같지만 다르게",
            _                           => throw new ArgumentOutOfRangeException(nameof(endingType), endingType, null)
        };
    }
    
    public static string GetName(this CutSceneType cutSceneType)
    {
        return cutSceneType switch
        {
            CutSceneType.Intro     => "Intro",
            CutSceneType.Chapter02 => "Chapter2",
            _                      => throw new ArgumentOutOfRangeException(nameof(cutSceneType), cutSceneType, null)
        };
    }
    
    public static string GetName(this Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => "Easy",
            Difficulty.Hard => "Hard",
            _               => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
        };
    }
}
