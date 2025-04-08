using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//"Menu배너에 쓰일 이름 / 하위 배너이름 / 씬이름 / 단축키
// %: Ctrl 또는 Cmd(Mac OS)
// ^: Ctrl
// #: Shift
// &: Alt
public class SceneChanger : Editor
{
    [MenuItem("Stars/Scene/Sample Scene &1")]  
    public static void ChangeSampleScene()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/01.Scenes/SampleScene.unity");
    }

    [MenuItem("Stars/Scene/Test Scene &1")] 
    public static void ChangeIntroScene()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/01.Scenes/TestScene.unity");
    }

    [MenuItem("Stars/Scene/CutScene_YDY &1")]
    public static void ChangeCutScene()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/01.Scenes/CutScene_YDY.unity");
    }

    
}


