# 변경 기록

## [0.3.0] - 2026-08-13

- **(Breaking)** 네임스페이스를 `Jeomseon.Data`(패키지 이름과 무관했음) → `Jeomseon.Unity.Json`으로
  변경했습니다. 워크스페이스 전체 네임스페이스 규칙(`AGENTS.md` 참고)을 적용한 것으로, 폴더 구조
  변경은 없습니다.

## [0.1.2] - 2026-07-29

- asmdef의 `rootNamespace`와 소스 파일 위치를 namespace에 맞게 정리했습니다.

## [0.1.1] - 2026-07-29

- JSON 목록 래퍼 타입을 확인하는 `Basic Usage` 샘플을 추가했습니다.

## [Unreleased]

## [0.2.0] - 2026-08-10

- `com.unity.nuget.newtonsoft-json` 의존성을 `3.0.2`(Json.NET 13.0.1)에서 `3.2.2`(Json.NET 13.0.2)로 상향했습니다.
- **수정**: Runtime·Tests·Samples asmdef가 존재하지 않는 asmdef 이름 `"Unity.Newtonsoft.Json"`을
  참조하고 있었습니다(이 패키지는 asmdef 없이 DLL만 배포됩니다). `overrideReferences` +
  `precompiledReferences: ["Newtonsoft.Json.dll"]`로 세 asmdef 모두 정정했습니다.
- **수정**: `JsonDataList<T>.Data`에 `[JsonIgnore]`가 누락되어 직렬화 결과에 `DataList`와 중복된
  `"Data"` 키가 함께 노출되던 결함을 수정했습니다.
- `JsonDataList<T>`의 null/빈 목록/중첩 타입(`Dictionary` 포함)/Unity 값 타입/스키마 불일치
  데이터에 대한 직렬화 계약 EditMode 테스트 10건을 추가했습니다. `Vector3`처럼 자기 참조형
  프로퍼티(`normalized`)를 가진 Unity 값 타입은 역직렬화(읽기)만 지원하며, 직렬화(쓰기)하면
  `JsonSerializationException`이 발생함을 테스트와 README에 명시했습니다.
- **결정**: `JsonDataList<T>`는 JsonUtility로 대체하지 않고 Newtonsoft.Json 의존을 유지합니다.
  `DataList`가 auto-property라 JsonUtility(필드만 직렬화)로는 애초에 직렬화되지 않으며,
  `T`에 `Dictionary`를 포함한 중첩 모델을 담는 사용이 계약 테스트로 확정되어 있습니다.
- `JsonDataList<T>`에 생성자·setter가 `internal`인 이유, `Data`의 null/빈 목록 구분,
  오류 시 발생하는 예외를 설명하는 XML 문서 주석을 추가하고, 한·영 README에 사용법과 동작
  계약 섹션을 추가했습니다.
- `Basic Usage` Sample(`JsonSample`)에 정상 역직렬화, null/빈 목록 구분, 타입 불일치 오류
  시연 컨텍스트 메뉴를 추가하고, `JsonSample`이 부착된 `JsonSample.unity` Scene 자산을
  커밋했습니다(README로 사용자가 직접 GameObject를 구성하게 하는 방식은 AGENTS.md 정책상
  허용하지 않습니다).
- 정적 이벤트와 전역 인스턴스의 Domain Reload 비활성화 호환성을 검토합니다.

## [0.1.0] - 2026-07-29

- JeomseonScriptPack의 관련 모듈을 독립 UPM 패키지로 분리했습니다.


## [0.1.3] - 2026-08-05

- Unity 6000.5.7f1을 최소 지원 버전으로 상향했습니다.
