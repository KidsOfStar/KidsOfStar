using GoogleSheet.Core.Type;

#region Dialogue

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
    People1,
    People2,
    People3,
    Women,
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
    UpdateProgress,
    ChangeForm,
    GoToEnding,
}

[UGS(typeof(DialogActionType))]
public enum CustomActionType
{
    GoToEnding,
    MoveTo,
    PlayCutScene,
    Return,
}

public enum DialogCustomMethodType
{
    ChangeHumanForm,
}

#endregion

#region Sound

public enum BgmSoundType
{
    Maorum,
    MaorumChase,
    InForest,
    InForestPuzzle,
    WithDogs,
    WithDogsRun,
}

public enum SfxSoundType
{
}

public enum FootstepType
{
}

#endregion

#region Stage

public enum SceneType
{
    Title,
    Loading,
    Chapter1,
    Chapter1Puzzle,
    Chapter2,
    Chapter202,
    Chapter3,
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

public enum InteractionType
{
    StartGame,
    EndGame
}

#endregion

#region CutScene

public enum EndingType
{
    // 기본 엔딩
    Absorb,
    Stable,
    Obedience,
    Adaptation,
    Mistake,

    // 중요 엔딩
    ComfortableLife,
    WinRecognition,
    DreamingCat,
    IntroTheOcean,
    DifferentButSame,
}

public enum CutSceneType
{
    Intro,
    Rescued,
    DaunRoom,
    LeavingForest,
    DogFormChange,
    FieldNormalLife,
    SemyungGoOut,
    JigimOrder,
    RescueKitten,
}

#endregion

#region Setting

public enum UIPosition
{
    UI,    // 기본 씬 UI
    Popup, // 팝업 창
    Top,   // 에러 창 
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

public enum WarningType
{
    None,
    Squirrel,
    BoxMissing,
    BoxFalling,
}

#endregion