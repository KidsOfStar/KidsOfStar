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
    Loading,
    Main,
}
