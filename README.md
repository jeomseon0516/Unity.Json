# Jeomseon Unity JSON

한국어 | [English](README.en.md)

Newtonsoft.Json으로 읽은 DataList JSON 프로퍼티를 IReadOnlyList로 노출하는 작은 컨테이너입니다.

## 빠르게 시작하기

JsonConvert.DeserializeObject<JsonDataList<T>>(json) 호출 → 결과 Data 확인 → 읽기 전용 목록 소비. 생성자와 DataList setter는 internal입니다.

전체 샘플 코드: [JsonSample.cs](Samples~/BasicUsage/JsonSample.cs).

```csharp
using Jeomseon.Unity.Json;
using Newtonsoft.Json;
using UnityEngine;

namespace Jeomseon.Samples.Json
{
    public sealed class JsonSample : MonoBehaviour
    {
        [ContextMenu("1. 정상 역직렬화")]
        private void DeserializeNormalList()
        {
            var result = JsonConvert.DeserializeObject<JsonDataList<string>>("{\"DataList\":[\"a\",\"b\"]}");
            Debug.Log($"Data.Count = {result.Data.Count}, [0] = {result.Data[0]}, [1] = {result.Data[1]}");
        }

        [ContextMenu("2. null 목록과 빈 목록 구분")]
        private void DistinguishNullFromEmpty()
        {
            var missingKey = JsonConvert.DeserializeObject<JsonDataList<string>>("{}");
            var emptyArray = JsonConvert.DeserializeObject<JsonDataList<string>>("{\"DataList\":[]}");
            Debug.Log($"DataList 키 없음 -> Data == null: {missingKey.Data == null}");
            Debug.Log($"빈 배열([]) -> Data == null: {emptyArray.Data == null}, Data.Count: {emptyArray.Data.Count}");
        }

        [ContextMenu("3. 오류 데이터: 타입 불일치")]
        private void DeserializeTypeMismatch()
        {
            try
            {
                JsonConvert.DeserializeObject<JsonDataList<int>>("{\"DataList\":[1,\"not-a-number\",3]}");
                Debug.LogWarning("예외 없이 성공했습니다 (예상 밖 결과).");
            }
            catch (JsonReaderException ex)
            {
                Debug.Log($"예상대로 예외 발생: {ex.GetType().Name}");
            }
        }

        [ContextMenu("4. 지원 타입 확인")]
        private void PrintSupportedType()
        {
            Debug.Log($"JSON 목록 래퍼: {typeof(JsonDataList<string>).FullName}");
        }
    }
}
```

## 계약과 주의사항

JSON의 DataList 누락/null은 Data==null이고 []는 Count==0입니다. 읽기 전용 목록이 원소 객체의 불변성까지 보장하지 않습니다. 잘못된 JSON과 타입 불일치는 Newtonsoft 역직렬화 예외로 처리합니다.

## 요구 사항

- Unity `6000.6.0f1`
- 패키지: `com.jeomseon.unity.json` `0.3.1`
- `com.unity.nuget.newtonsoft-json`: `3.2.2`

## 설치

프로젝트 `Packages/manifest.json`의 기존 dependencies와 registry를 유지하면서 다음 항목을 병합합니다.

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.jeomseon"
      ]
    }
  ],
  "dependencies": {
    "com.jeomseon.unity.json": "0.3.1"
  }
}
```

또는 Package Manager의 **Install package from git URL**을 사용합니다:

```text
https://github.com/jeomseon0516/Unity.Json.git#v0.3.1
```

## API 사용과 상세 설명

### 사용법

`JsonDataList<T>`는 Newtonsoft.Json으로 역직렬화한 JSON 배열을 담는 읽기 전용 컨테이너입니다.
생성자와 내부 저장 필드가 모두 `internal`이라 인스턴스는 `JsonConvert.DeserializeObject<T>`로만 만들어집니다.

```csharp
using Jeomseon.Data;
using Newtonsoft.Json;

var result = JsonConvert.DeserializeObject<JsonDataList<string>>(json);
IReadOnlyList<string> items = result.Data;
```

### 동작 계약

- 원본 JSON에 `DataList` 키가 없거나 값이 명시적으로 `null`이면 `Data`도 `null`입니다.
- 빈 배열(`[]`)은 `null`이 아닌 빈 컬렉션(`Count == 0`)으로 구분됩니다.
- 알 수 없는 필드가 섞인 JSON은 무시하고 역직렬화합니다(전방 호환).
- 원소 타입이 `T`와 맞지 않거나 JSON 자체가 잘못되면 `Newtonsoft.Json.JsonReaderException`이 발생하며,
  부분적으로 채워진 인스턴스는 반환되지 않습니다.
- `T`에는 `Dictionary`, 중첩 클래스 등 Newtonsoft.Json이 지원하는 임의의 타입을 사용할 수 있습니다.
- **`Vector3`처럼 자기 참조형 프로퍼티(예: `normalized`가 다시 `Vector3`를 반환)를 가진 Unity 값
  타입은 역직렬화(읽기)만 지원합니다.** 이런 타입을 담은 `JsonDataList<T>`를 `JsonConvert.SerializeObject`로
  직렬화(쓰기)하면 `JsonSerializationException`("Self referencing loop detected")이 발생합니다.

## 샘플 실행

**Package Manager > Samples**에서 예제를 Import합니다. 포함된 Scene 또는 Editor 도구를 열고 필요한 Setup 메뉴를 먼저 실행합니다.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [JsonSample](Samples~/BasicUsage/JsonSample.unity)

## 테스트

[테스트 코드](Tests)

manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 EditMode/PlayMode 어셈블리를 실행합니다.

## 추가 문서

[전체 코드 지도와 패키지 구성](Documentation~/Overview.md)
- [Changelog](CHANGELOG.md)

## 라이선스

[License](LICENSE.md)
