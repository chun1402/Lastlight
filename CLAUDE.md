Lastlight 프로젝트의 루트(C:\\git\_Clone\\Lastlight)에 CLAUDE.md 파일을 만들어줘.



먼저 Assets/\_Project/Scripts/Player/PlayerController.cs 를 읽고 우리 코딩 스타일을 분석해서 반영해줘.



다음 내용을 포함:



\## 프로젝트 개요

\- 이름: Lastlight

\- 장르: 1인칭 웨이브 슈터 (Vampire Survivors의 1인칭 버전)

\- 비주얼: 로우폴리 스타일라이즈드, 디스토피아 톤

\- 목표: 2주 안에 GitHub 포트폴리오용 완성품



\## 기술 스택

\- Unity 6 LTS (6000.4.x)

\- URP (Universal Render Pipeline)

\- New Input System (Project-wide Actions 사용)

\- C# (.NET Standard 2.1)



\## 폴더 구조

Assets/

├── \_Project/                       # 내 작업물

│   ├── Scripts/

│   │   ├── Player/                 # PlayerController 등

│   │   ├── Weapons/                # 무기 시스템 (Day 3)

│   │   ├── Enemies/                # 적 AI (Day 4)

│   │   ├── Managers/               # GameManager, WaveManager

│   │   ├── UI/                     # HUD, 메뉴

│   │   ├── Systems/                # ObjectPool, SaveSystem 등

│   │   └── Interfaces/             # IDamageable 등

│   ├── ScriptableObjects/

│   │   ├── Weapons/

│   │   ├── Enemies/

│   │   └── Upgrades/

│   ├── Prefabs/

│   ├── Scenes/                     # Main.unity

│   ├── Materials/

│   └── Audio/

└── ThirdParty/                     # Asset Store 에셋 (.gitignore)



\## 코딩 컨벤션

\- namespace: Lastlight.{카테고리} (예: Lastlight.Player, Lastlight.Weapons)

\- private 필드는 camelCase

\- public 필드 대신 \[SerializeField] private 사용

\- Animator 파라미터는 Animator.StringToHash로 캐싱

\- 입력 처리: InputSystem.actions.FindAction("ActionName") 패턴

\- 이벤트 기반 입력은 OnEnable/OnDisable에서 구독/해제



\## 아키텍처 패턴

\- 데이터 기반 설계: ScriptableObject로 무기/적/업그레이드 분리

\- 인터페이스 기반: IDamageable, IPickupable 등

\- 오브젝트 풀링: 총알/적 재활용

\- 이벤트 기반 통신: 매니저 간 결합도 낮춤



\## 외부 에셋

\- Low Poly Soldiers Demo (Polygon Blacksmith) - 플레이어 모델

\- Low Poly Shooter Pack Free Sample (Infima Games) - 1인칭 무기 (예정)

\- 모든 외부 에셋은 Assets/ThirdParty/ 안에, Git ignore됨



\## 진행 상황

\- \[x] Day 1: 프로젝트 세팅 (URP, 폴더 구조)

\- \[x] Day 2: FPS 캐릭터 컨트롤러

\- \[x] Day 2.5: 플레이어 모델 + 척추 본 IK

\- \[ ] Day 3: 무기 시스템 (피스톨, ScriptableObject)

\- \[ ] Day 4: 적 AI (NavMesh + FSM)

\- \[ ] Day 5\~7: 웨이브, 업그레이드, UI, 사운드

\- \[ ] Day 8\~14: 폴리시 + 빌드 배포



\## 작업 시 주의사항

\- Bip001 본 시스템은 3ds Max 스타일 (Z축이 피치 회전)

\- 1인칭 시야에서 자기 모델은 Layer Mask로 제외 (Player Layer)

\- 외부 에셋은 절대 Git에 커밋하지 않음 (라이선스 위반)

