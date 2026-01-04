# 🎮 Unity 2D Platformer Project
> **유니티 6의 최신 물리 시스템(`linearVelocity`)과 싱글톤 아키텍처를 활용한 데이터 중심 설계 프로젝트**

---

## 1️⃣ 프로젝트 개요
* **프로젝트 성격**: 2D 게임의 핵심 시스템(물리 기반 조작, 씬 데이터 관리, AI)의 기초를 다지고 최적화된 아키텍처를 설계하는 프로젝트
* **핵심 목표**: 
  * `linearVelocity`를 이용한 정교한 캐릭터 피지컬 컨트롤
  * 싱글톤 패턴과 `DontDestroyOnLoad`를 조합한 영속적 데이터 시스템 구축
  * 시네머신 및 타일맵 최적화를 통한 완성도 높은 레벨 디자인

---

## 2️⃣ 사용 기술 (Tech Stack)
| 분류 | 상세 내용 |
| :--- | :--- |
| **Engine** | **Unity 6 (6000.2.12f1)** |
| **Language** | C# |
| **Physics** | Rigidbody2D (`linearVelocity`), Composite Collider 2D, Physics Material 2D |
| **Architecture** | **Singleton Pattern** (GameSession, ScenePersist) |
| **Modules** | Input System, Cinemachine (Confiner 2D), TextMeshPro |

---

## 3️⃣ 주요 기능 및 핵심 코드

### 📊 Core Script Analysis
프로젝트의 각 기능을 책임지는 핵심 스크립트와 적용된 기술적 포인트입니다.

| Class | Responsibility | Key Technique |
| :--- | :--- | :--- |
| **`PlayerMovement.cs`** | 플레이어 이동 및 상태 제어 | Input System, `linearVelocity`, `LayerMask` |
| **`GameSession.cs`** | 전역 게임 데이터 관리 | Singleton, `DontDestroyOnLoad`, UI Update |
| **`ScenePersist.cs`** | 씬 내부 데이터(아이템) 유지 | Singleton, Reset Logic |
| **`Bullet.cs`** | 투사체 물리 및 충돌 처리 | `localScale` 연동 발사 방향 결정 |
| **`EnemyMovement.cs`** | 적 NPC AI 및 정찰 | Edge Detection (`OnTriggerExit2D`) |
| **`CoinPickUp.cs`** | 아이템 획득 및 사운드 재생 | `AudioSource.PlayClipAtPoint` (사운드 생존 로직) |
| **`LevelExit.cs`** | 레벨 클리어 및 다음 씬 로드 처리 | StartCoroutine, SceneManager, ResetScenePersist |

---

### 💡 핵심 구현 로직

#### **1. 물리 기반 캐릭터 컨트롤 (`PlayerMovement.cs`)**
수평 이동 시 현재의 Y축 속도를 유지하도록 설계하여, 물리 연산 중 중력이나 점프 힘이 0으로 덮어씌워지는 현상을 방지했습니다.

```csharp
void Run() {
    // 중력(y)을 보존하며 수평(x) 속도만 제어
    Vector2 playerVelocity = new Vector2(moveInput.x * MoveSpeed, myRigidbody.linearVelocity.y);
    myRigidbody.linearVelocity = playerVelocity;
    
    // 이동 여부에 따른 애니메이션 불리언 설정
    bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
    myAnimator.SetBool("isRunnung", hasHorizontalSpeed);
}
```
#### 2. 데이터 영속성 시스템 (GameSession.cs)
싱글톤 패턴을 적용하여 씬이 로드될 때 기존 데이터(점수, 목숨)를 안전하게 보호합니다.

```csharp
void Awake() {
    int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
    if (numberGameSessions > 1) {
        Destroy(gameObject); // 중복 생성 방지
    } else {
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
    }
}
```
---

## 4️⃣ 트러블슈팅 (Issues & Solutions) 🚀
### 🔍 캐릭터가 벽에 붙어 멈추는 현상

현상: 벽을 향해 이동하며 점프 시 마찰력으로 인해 벽에 고정됨.

해결: 마찰력이 0인 Physics Material 2D를 캐릭터 콜라이더에 적용하여 `linearVelocity` 기반의 원활한 이동 확보.

### 🔍 객체 파괴 시 효과음 소실

현상: 아이템 획득 시 Destroy()가 호출되어 오디오 소스가 즉각 사라짐.

해결: `AudioSource.PlayClipAtPoint`에 `transform.position` 을 추가하여 객체의 파괴와 독립적으로 동작하는 사운드 재생 환경 구축.

### 🔍 지형 끝단(Cliff) 감지 로직

현상: 적 NPC가 바닥이 없는 곳으로 계속 직진하여 추락.

해결: `OnTriggerExit2D`를 지형 감지용 자식 오브젝트에 적용하여 지형을 벗어나는 즉시 속도를 반전시켜 정찰 루프 구현.

---

## 5️⃣ 제작자 정보
제작: M1nu4286
강의 출처: Udemy - Complete C# Unity Game Developer 2D (GameDev.tv)
학습 키워드: Singleton Architecture, Linear Velocity, Scene Persistence, Edge Detection AI