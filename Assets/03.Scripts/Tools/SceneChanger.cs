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
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/SampleScene.unity");
    }

    [MenuItem("Stars/Scene/Test Scene &1")] 
    public static void ChangeIntroScene()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/TestScene.unity");
    }

}
