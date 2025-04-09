using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class FormData
{
    // 형태 이름
    [SerializeField] private string formName;
    public string FormName { get { return formName; } }
    // 현재 형태의 스프라이트 이미지
    [SerializeField] private Sprite formImage;
    public Sprite FormImage { get { return formImage; } }
    // 애니메이터 컨트롤러
    // 애니메이션 추가하기 전까지는 비활성화
    //[SerializeField] AnimatorController animCon;
    //public AnimatorController AnimCon { get { return animCon; } }
    // 점프력
    [SerializeField] private float jumpForce;
    public float JumpForce { get { return jumpForce; } }
    // 크기
    [SerializeField] private float sizeX;
    public float SizeX { get { return sizeX; } }
    [SerializeField] private float sizeY;
    public float SizeY { get { return sizeY; } }
    // 힘
    [SerializeField] private float force;
    public float Force { get { return force; } }
    // 무게
    [SerializeField] private float weight;
    public float Weight { get { return weight; } }
    // 해금 상태
    public bool IsActive = false;
}

[CreateAssetMenu(fileName = "new PlayerForm Data", menuName = "PlayerForm Data")]
public class PlayerFormData : ScriptableObject
{
    [SerializeField] private List<FormData> playerFromDataList = new List<FormData>();
    public List<FormData> PlayerFromDataList { get { return playerFromDataList; } }
}