# Jeomseon Unity JSON

[한국어](README.md) | English

A small container exposing the DataList JSON property as IReadOnlyList after Newtonsoft.Json deserialization.

## Quick start

Call JsonConvert.DeserializeObject<JsonDataList<T>>(json), inspect Data, and consume the read-only list. Its constructor and DataList setter are internal.

Complete sample: [JsonSample.cs](Samples~/BasicUsage/JsonSample.cs).

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

## Contracts and limitations

Missing/null DataList gives Data==null; [] gives Count==0. Read-only list access does not make element objects immutable. Handle Newtonsoft exceptions for invalid JSON/type mismatches.

## Requirements

- Unity `6000.6.0f1`
- Package: `com.jeomseon.unity.json` `0.3.1`
- `com.unity.nuget.newtonsoft-json`: `3.2.2`

## Installation

Merge this into your project's `Packages/manifest.json`; preserve existing dependencies and registries.

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

Or use **Install package from git URL** in Package Manager:

```text
https://github.com/jeomseon0516/Unity.Json.git#v0.3.1
```

## API usage and details

### Usage

`JsonDataList<T>` is a read-only container for a JSON array deserialized with Newtonsoft.Json.
Its constructor and backing field are both `internal`, so instances are only created through `JsonConvert.DeserializeObject<T>`.

```csharp
using Jeomseon.Data;
using Newtonsoft.Json;

var result = JsonConvert.DeserializeObject<JsonDataList<string>>(json);
IReadOnlyList<string> items = result.Data;
```

### Contract

- `Data` is `null` when the source JSON has no `DataList` key or the value is explicitly `null`.
- An empty array (`[]`) is distinguished from `null`: it results in a non-null, empty collection (`Count == 0`).
- Unknown fields in the JSON are ignored (forward compatible).
- A type mismatch between an element and `T`, or malformed JSON, throws `Newtonsoft.Json.JsonReaderException`;
  a partially populated instance is never returned.
- `T` may be any type Newtonsoft.Json supports, including `Dictionary` and nested classes.
- **Unity value types with self-referential properties (e.g. `Vector3.normalized`, which itself returns
  a `Vector3`) are only supported for deserialization (reading).** Serializing (writing) a
  `JsonDataList<T>` containing such a type via `JsonConvert.SerializeObject` throws
  `JsonSerializationException` ("Self referencing loop detected").

## Samples

Import the desired entry from **Package Manager > Samples**. Use the included Scene/Editor tool and complete its Setup step first, if required.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [JsonSample](Samples~/BasicUsage/JsonSample.unity)

## Tests

[Test sources](Tests)

Add the package ID to manifest `testables`, then execute its relevant EditMode/PlayMode assemblies in Unity Test Runner.

## Further reading

[Complete source map and package metadata](Documentation~/Overview.en.md)
- [Changelog](CHANGELOG.md)

## License

[License](LICENSE.md)
