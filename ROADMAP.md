# Json 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## 작업 순서

1. **P0-01 — Newtonsoft.Json 의존성 버전 재정리 (완료)**
   - `com.unity.nuget.newtonsoft-json`을 `3.0.2`(Json.NET 13.0.1) → `3.2.2`(Json.NET 13.0.2)로 상향했습니다.
   - dll 실물을 직접 보유하지 않고 UPM 패키지 의존성으로만 참조하는 기존 방식을 유지합니다.
   - **추가 결함 수정**: `com.unity.nuget.newtonsoft-json`은 asmdef 없이 DLL만 배포하는데
     Runtime asmdef가 존재하지 않는 `"Unity.Newtonsoft.Json"`을 참조하고 있었습니다. 세
     asmdef(Runtime/Tests/Samples) 모두 `overrideReferences` + `precompiledReferences:
     ["Newtonsoft.Json.dll"]`로 정정했습니다.
2. **P1-01 — 직렬화 계약 명확화 (완료)**
   - `Tests/Editor/JsonDataListSerializationTests.cs`에 null, 빈 목록, 중첩 타입(`List`·`Dictionary` 포함),
     Unity 값 타입(`Vector3` public 필드), 알 수 없는 필드가 섞인 데이터, 타입이 맞지 않는 데이터,
     완전히 잘못된 JSON에 대한 계약 테스트를 추가했습니다.
   - **결함 발견 및 수정**: 읽기 전용 공개 프로퍼티 `Data`에 `[JsonIgnore]`가 없어 직렬화 시
     `DataList`와 동일한 값이 `"Data"` 키로 중복 노출되던 문제를 확인해 수정했습니다.
   - **실측으로 발견한 한계(사용자 Unity Test Runner 실행으로 확인, 2026-08-10)**: `Vector3`를
     그대로 직렬화하면 `normalized` 프로퍼티가 `Vector3`를 반환하고 그 결과의 `normalized`도
     다시 `Vector3`를 반환해 타입 구조상 종료되지 않아, Newtonsoft가 `JsonSerializationException`
     ("Self referencing loop detected")을 던집니다. `JsonDataList<T>`는 이런 자기 참조형
     프로퍼티를 가진 Unity 값 타입에 대해 **역직렬화(읽기)만 지원**하며 직렬화(쓰기)는 지원
     범위 밖입니다. 테스트를 `Deserialize_UnityValueType_PopulatesPublicFields`(정상 동작)와
     `Serialize_UnityValueTypeWithSelfReferentialProperty_Throws`(예상된 예외)로 분리해 이
     계약을 명시했습니다.
   - 확정된 계약: null/키 누락은 `Data == null`, 빈 배열은 `Data.Count == 0`(빈 것과 없는 것을 구분),
     알 수 없는 필드는 무시, 타입 불일치·잘못된 JSON은 `JsonReaderException`으로 즉시 실패(부분 손상 없음),
     자기 참조형 프로퍼티를 가진 타입의 직렬화는 `JsonSerializationException`으로 실패.
3. **P2-01 — JsonUtility와 Newtonsoft 역할 구분 (완료, 결정: Newtonsoft 유지)**
   - `JsonUtility`는 public 필드(또는 `[SerializeField]`)만 직렬화하며 프로퍼티는 인식하지 않습니다.
     `JsonDataList<T>.DataList`는 auto-property이므로 구조 자체를 필드로 바꾸지 않는 한 대체가 불가능합니다.
   - P1-01 계약 테스트가 `T`에 `Dictionary`를 포함한 중첩 모델을 담는 것을 정상 사용으로 확정했으며,
     `JsonUtility`는 `Dictionary` 직렬화를 지원하지 않아 기능 동등성을 만족하지 못합니다.
   - 두 사유 모두 Unity 대체 기준(기능·계약 동등성)을 충족하지 못해 Newtonsoft.Json 의존을 유지합니다.
4. **P2-02 — 공개 모델 API 개선 (완료)**
   - `internal` 생성자·setter가 인스턴스 생성 경로를 `JsonConvert.DeserializeObject`로 한정한다는
     정책과, `Data`의 null/빈 목록 구분, 오류 시 `JsonReaderException`이 발생한다는 계약을
     XML 문서 주석과 한·영 README 사용법/동작 계약 섹션으로 문서화했습니다.
   - `IReadOnlyList<T> Data`는 기존 그대로 유지하며 별도의 API 변경(시그니처 변경)은 하지 않았습니다.
5. **P3-01 — Serializer 추상화**
   - 여러 구현이 실제로 필요해질 때만 선택 가능한 serializer 계약을 도입합니다.
