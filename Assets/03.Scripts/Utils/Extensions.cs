using System;

public static class Extensions
{
    public static string GetName(this BgmSoundType bgm)
    {
        return bgm switch
        {
            BgmSoundType.Maorum         => "Maorum",
            BgmSoundType.MaorumChase    => "MaorumChase",
            BgmSoundType.InForest       => "InForest",
            BgmSoundType.InForestPuzzle => "InForestPuzzle",
            BgmSoundType.WithDogs       => "WithDogs",
            BgmSoundType.WithDogsRun    => "WithDogs_Run",
            _                           => throw new ArgumentOutOfRangeException(nameof(bgm), bgm, null)
        };
    }

    public static string GetName(this SfxSoundType sfx)
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
            SceneType.Title          => "TitleScene",
            SceneType.Loading        => "LoadingScene",
            SceneType.Chapter1       => "Chapter_1",
            SceneType.Chapter1Puzzle => "Chapter_103",
            SceneType.Chapter2       => "Chapter_2",
            SceneType.Chapter202     => "Chapter_202",
            SceneType.Chapter3       => "Chapter_3",
            _                        => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
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
            EndingType.Absorb           => "흡수",
            EndingType.Stable           => "안정",
            EndingType.Obedience        => "복종",
            EndingType.Adaptation       => "적응",
            EndingType.Mistake          => "실수",
            _                           => throw new ArgumentOutOfRangeException(nameof(endingType), endingType, null)
        };
    }
    
    public static string GetName(this CutSceneType cutSceneType)
    {
        return cutSceneType switch
        {
            CutSceneType.Intro     => "Intro",
            CutSceneType.Rescued   => "Rescued",
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
