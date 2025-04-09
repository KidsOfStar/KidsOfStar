using UnityEngine;

[CreateAssetMenu(fileName = "LoadingData", menuName = "ScriptableObject/LoadingData")]
public class LoadingData : ScriptableObject
{
    [Header("Backgrounds")]
    public Sprite[] Backgrounds;
    
    [Header("Tooltips")]
    public string[] Tooltips;
}
