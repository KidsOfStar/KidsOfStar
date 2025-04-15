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
}

[UGS(typeof(DialogActionType))]
public enum DialogActionType
{
    None,
    ShowSelect,
    DataSave,
    ModifyTrust,
}

public enum SoundType
{
    
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

public enum EndingType
{
    // 임시 (나중에 기획자님이 이름 지어주실 예정임)
    Ending01,
    Ending02,
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
