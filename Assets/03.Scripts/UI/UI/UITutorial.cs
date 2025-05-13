using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private RectTransform holeMask;
    
    // timeScale = 0
    // 강조 할 ui 찾음 -> hole x,y = 강조 할 ui 위치
    // button 추가
    // 애니메이션?
    // 애니메이션 이후 블링크 화살표 active < offset 설정 (타겟 ui 조금 위쪽)
    //  ㄴ 화면을 반으로 나눠서 왼쪽 섹션이면 플립, 아니면 그대로
    // hole size = ui의 크기 + padding
}
