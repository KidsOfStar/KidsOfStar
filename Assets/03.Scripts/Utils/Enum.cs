using GoogleSheet.Core.Type;

[UGS(typeof(CharacterType))]
public enum CharacterType
{
    Maorum,
    WaterPlant1,
    WaterPlant2,
    Daun,
    Dongdo,
    Jigim,
    Semyung,
    Hanyung,
    Ea,
    Hani,
    Jieui,
    Bihyi,
    Stone,
    Songsari,
    Dolmengee,
    People,
}

[UGS(typeof(DialogActionType))]
public enum DialogActionType
{
    None,
    ShowSelect,
    DataSave,
    ModifyTrust,
    PlayCutScene,
    LoadScene,
}

public enum SoundType
{
    UiButton,
    
}

public enum FootstepType
{
    
}

public enum SceneType
{
    Title,
    Loading,
    Chapter01,
}

public enum ChapterType
{
    Chapter01,
    Chapter02,
    Chapter03,
}

public enum Difficulty
{
    Easy,
    Hard,
}

public enum EndingType
{
    ComfortableLife,
    WinRecognition,
    DreamingCat,
    IntroTheOcean,
    DifferentButSame,
}

public enum CutSceneType
{
    Intro,
    Chapter02,
}

public enum UIPosition
{
    UI,     // 기본 씬 UI
    Popup,  // 팝업 창
    Top,    // 에러 창 
}

public enum ObstacleType
{
    SmallSeaweed,
    MediumSeaweed,
    LargeSeaweed,
    Stone
}
public enum ReferenceMode //레터박스 화면비율 선정 Mode
{ 
    DesignedAspectRatio, 
    OrginalResolution 
};
