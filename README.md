# Jeomseon Unity JSON

JSON data container utilities using Newtonsoft.Json.

## 설치

OpenUPM 등록 전에는 Package Manager의 **Add package from git URL**에서 다음 주소를 사용합니다.

```text
https://github.com/jeomseon0516/Unity.Json.git#v0.1.1
```

## 사용법

`JsonDataList<T>`는 Newtonsoft.Json으로 역직렬화한 JSON 배열을 담는 읽기 전용 컨테이너입니다.
생성자와 내부 저장 필드가 모두 `internal`이라 인스턴스는 `JsonConvert.DeserializeObject<T>`로만 만들어집니다.

```csharp
using Jeomseon.Data;
using Newtonsoft.Json;

var result = JsonConvert.DeserializeObject<JsonDataList<string>>(json);
IReadOnlyList<string> items = result.Data;
```

## 동작 계약

- 원본 JSON에 `DataList` 키가 없거나 값이 명시적으로 `null`이면 `Data`도 `null`입니다.
- 빈 배열(`[]`)은 `null`이 아닌 빈 컬렉션(`Count == 0`)으로 구분됩니다.
- 알 수 없는 필드가 섞인 JSON은 무시하고 역직렬화합니다(전방 호환).
- 원소 타입이 `T`와 맞지 않거나 JSON 자체가 잘못되면 `Newtonsoft.Json.JsonReaderException`이 발생하며,
  부분적으로 채워진 인스턴스는 반환되지 않습니다.
- `T`에는 `Dictionary`, 중첩 클래스 등 Newtonsoft.Json이 지원하는 임의의 타입을 사용할 수 있습니다.
- **`Vector3`처럼 자기 참조형 프로퍼티(예: `normalized`가 다시 `Vector3`를 반환)를 가진 Unity 값
  타입은 역직렬화(읽기)만 지원합니다.** 이런 타입을 담은 `JsonDataList<T>`를 `JsonConvert.SerializeObject`로
  직렬화(쓰기)하면 `JsonSerializationException`("Self referencing loop detected")이 발생합니다.

## 리팩토링 방침

Unity가 제공하는 동등 기능과 비교해 대체 가능한 코드는 소스의 한글 TODO 주석과 CHANGELOG의 Unreleased 항목에서 추적합니다.
