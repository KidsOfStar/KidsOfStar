# KidsOfStar
# 🧭 프로젝트 개요

<details>
<summary>클릭해서 열기</summary>

- **프로젝트 이름**: Kids of Star
- **개발 기간**: 2025.04.07 ~ 
- **팀 구성**: 강현아(팀장/기획), 윤동영(개발), 김자은(개발), 김태겸(개발), 김혜지(개발)
- **사용 기술**: Unity, C#, Git
- **장르/타겟 플랫폼**: 2D, 퍼즐, 플랫포머, 어드벤처 / 모바일
- **한 줄 소개**: 퍼즐을 풀고 삶의 목적을 찾는 게임

</details>

---

# 🎮 게임 소개

<details>
<summary>클릭해서 열기</summary>

- **게임 설명**: 플레이어가 형태변화를 이용해 목표 지점으로 나아가는 힐링 모바일 게임
- **기획 배경**: 삶에 지친 현대인들에게 위로를 주기 위해 기획
- **주요 기능/콘텐츠**:
    - 형태 변화 : 
    - 퍼즐
    - 대사 시스템
    - 

</details>

---

# ✨ 주요 특징 및 기술 구현

<details>
<summary>클릭해서 열기</summary>

### 🔧 기술 스택
- Unity 버전: 2022.3.17f1
- 주요 플러그인 / 외부 라이브러리: 
    - Cinemachine: 카메라 제어
    - TextMeshPro: 텍스트 렌더링
    - Unity Google Sheets: 구글 시트 연동
    - Ingame Debug Console: 디버깅 툴

### 🧠 주요 기능 구현
<details>
<summary>🔧 회전 퍼즐 시스템 (윤동영)</summary>

### 📝 기능 설명
회전 퍼즐 게임은 다채로운 컨텐츠를 위해서 만든 이미지맞추기 형태의 미니게임

### ⚙️ 핵심 구현 포인트
“평탄화(flattened)”된 1차원 리스트를 인덱스 계산으로 2차원처럼 활용한 형태로
스크립터블 오브젝트를 활용한 데이터를 통해서 퍼즐의 이미지를 확장성 있게 구성하고,
퍼즐 조각을 프리펩화하여 그리드(1차원 리스트) 형태 + 그리드 너비(gridWidth)를 이용한 구조

</details>

<details>
<summary>🔧 나뭇잎 트램펄린 기믹 (윤동영)</summary>

### 📝 기능 설명
나뭇잎 점프(Leaf Jump)는 게임 내에서 일종의 트램펄린 역할을 하는 오브젝트

### ⚙️ 핵심 구현 포인트
충돌감지를 통해 ILeafJumpable 인터페이스를 구현한다면 점프동작을 위임하고, 인터페이스 구현 객체에서 실제 물리 처리를 수행하는 구조
인터페이스의 분리 덕분에, 플레이어·박스·기타 오브젝트 모두 같은 메서드만 구현하면 잎사귀 점프에 연동 가능

<img src="https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FcJ9Z6b%2FbtsNGVK7VbU%2Fr5zEWzDKmW4U8oNM0z39PK%2Fimg.png" width="400"/>

</details>

<details>
<summary>🔧 상자 옮기기 기믹 (윤동영)</summary>

### 📝 기능 설명

### ⚙️ 핵심 구현 포인트
플레이어의 Form형태에 따라서 형태 별 PushPower에 따라 박스를 미는 힘이 달리지는 구조로
IWeightable의 구현을 통해 boxWeight를 반환하고 RigidBody를 통해 실제 물리를 적용
박스가 플레이어와 부딪히며 튕기는 현상을 방지하기 위해,  코루틴 기반 충돌 무시로 부자연스러운 반발 제거

</details>

<details>
<summary>🔧 컷씬 시스템 (김자은)</summary>

### 📝 기능 설명
스토리의 일부를 컷신 형태로 구성하여, 애니메이션과 연출을 통해 **스토리 몰입감을 높이고 게임의 서사를 풍부하게 전달**합니다.

### ⚙️ 핵심 구현 포인트
- Unity Timeline을 활용해 다양한 연출을 자연스럽게 구성
- Signal Asset을 사용하여 타임라인 내부에서 **대사 출력, Bgm 전환, 컷신 종료** 등 이벤트를 정확한 타이밍에 실행
- 기능 단위 컴포넌트(DialogPlayer, BgmPlayer 등)를 타임라인과 연동하여 **타이밍 기반 제어** 수행

### 🧩 구조 및 연동
- 각 컷신 프리팹은 공통 기반 클래스 `CutSceneBase`를 가지고 있으며, 자체 Timeline(PlayableDirector)을 실행하는 구조
- 컷신 프리팹은 필요한 기능만 선택적으로 추가하는 **조립식 구조**로 구성
- 전체 컷신의 생성 및 실행 흐름은 `CutSceneManager`가 담당하며, 씬 내 컷신의 시작과 종료를 제어
</details>

<details>
<summary>🔧 대사 시스템 (김자은)</summary>

### 📝 기능 설명
컷씬 진행 중이거나 플레이어의 자유 상호작용 중에 **대사를 출력하고, 그에 따른 게임 내 액션을 트리거하는 시스템**입니다.  
스토리 흐름과 상호작용을 자연스럽게 연결해주는 핵심 역할을 합니다.

### ⚙️ 핵심 구현 포인트
- **Google Sheets 기반 데이터 테이블**을 Unity에 연동하여 대사 내용을 관리
- 각 대사는 `index`를 기준으로 불러오며, `nextIndex` 값을 통해 자동으로 다음 대사로 이어짐
- `ActionType` 필드를 통해 **대사 직후에 발생할 이벤트(선택지 표시, 컷씬 재생, 진행도 갱신 등)를 정의**

### 🧩 구조 및 연동
- 대사의 시작/종료, 분기 로직 관리는 `DialogueManager`에서 담당
- 출력은 `UITextBubble`이 담당하며, 대사 출력 자체에만 집중
- 각 대사 액션은 `IDialogActionHandler` 인터페이스를 구현한 클래스로 분리되어 있으며,  
  `Dictionary<DialogActionType, IDialogActionHandler>`를 통해 타입에 따라 동적으로 실행

</details>

<details>
<summary>🔧 UI 구조 (김태겸)</summary>

### 📝 기능 설명
게임 내 모든 UI는 Canvas 기반으로 구성되며, UI를 **기본 UI / 팝업 / 최상위 알림**의 세 계층으로 구분하여 관리합니다.  
이를 통해 사용자에게 지속적으로 보여져야 할 정보와, 특정 이벤트에 의한 임시 인터페이스를 **명확하게 분리**할 수 있습니다.

- **UI**: 조이스틱, 타이머, 점수 등 항상 표시되는 기본 UI
- **Popup**: 설정창, 일시정지, 결과창 등 이벤트 기반 인터페이스
- **Top**: 경고창, 시스템 오류 등 최우선 처리 인터페이스

### ⚙️ 핵심 구현 포인트
- UIManager에서 **Canvas 계층을 관리**하며, 각 계층에 맞는 UI Prefab을 동적으로 생성 및 제거
- Popup 계층은 중첩 표시를 고려한 **레이어 구조**로 설계 (예: 스택 또는 큐 구조)
- 최상위 알림 UI는 항상 다른 팝업 위에 표시되도록 **Sorting Order** 또는 **Transform 계층 구조**를 명확히 분리

### 🧩 구조 및 연동
- UIManager는 각 UI 계층별 Transform을 미리 참조하고 있으며, 필요 시 해당 위치에 프리팹을 Instantiate하여 붙이는 방식으로 동작
- 각 UI는 **인터페이스나 공통 베이스 클래스**를 상속하여 열기/닫기 등의 동작을 통일
- 게임의 상태 변화(예: 게임 오버, 일시정지)는 UIManager를 통해 해당 UI를 요청하는 방식으로 일관성 있게 처리됨

</details>

<details>
<summary>🔧 타임어택 미니게임 시스템 (김태겸)</summary>
(내용 작성)
</details>

<details>
<summary>🔧 플레이어 조작 (김혜지)</summary>
(내용 작성)
</details>

<details>
<summary>🔧 형태변화 시스템 (김혜지)</summary>
(내용 작성)
</details>

</details>

---

# 🧩 아키텍처 / 구조

<details>
<summary>클릭해서 열기</summary>

- **씬 구성 및 흐름**:
- **매니저 구조**: 싱글턴 구조 여부, Init 방식 등
- **데이터 관리**: ScriptableObject, JSON, CSV 등
- **이벤트 흐름**: UnityEvent, Observer 패턴 등

</details>

---

# 🤝 협업 방식

<details>
<summary>클릭해서 열기</summary>

- **Git 전략**: 브랜치 규칙, 병합 방식
- **이슈/업무 관리**: Trello, Notion 등
- **커밋 컨벤션**: 예) feat / fix / chore 등
- **회의/피드백 방식**: 회의 주기, 피드백 방식

</details>

---

# 🧪 테스트 및 디버깅

<details>
<summary>클릭해서 열기</summary>

- 테스트 방식:
- 자주 발생한 버그 및 해결 방법:
- 디버깅 방법:

</details>

---

# 🎬 시연 영상 및 이미지

<details>
<summary>클릭해서 열기</summary>

- YouTube 링크:
- 스크린샷:

</details>

---

# 📦 실행 방법

<details>
<summary>클릭해서 열기</summary>

- Unity 버전:
- 실행 전 주의사항:
- 빌드 방법:

</details>

---

# 💡 회고 / 느낀 점

<details>
<summary>클릭해서 열기</summary>

- 팀원별 회고 (예시):
    - 멤버1: "유니티 타임라인과 대사 시스템을 연동하면서 성장했습니다."
    - 멤버2: "팀으로 커뮤니케이션하며 완성도 높은 결과물을 만든 점이 뿌듯했습니다."
- 어려웠던 점 / 극복 과정:
- 다음에 더 잘하고 싶은 점:

</details>

---

# 📄 라이선스 / 참고 자료

<details>
<summary>클릭해서 열기</summary>

- 라이선스 유형: MIT, GPL 등
- 참고한 외부 문서, 튜토리얼, 이미지 출처 등

</details>