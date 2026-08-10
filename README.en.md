# Jeomseon Unity JSON

JSON data container utilities using Newtonsoft.Json.

## Installation

Before OpenUPM registration, use **Add package from git URL** in Package Manager with the following address.

```text
https://github.com/jeomseon0516/Unity.Json.git#v0.1.1
```

## Usage

`JsonDataList<T>` is a read-only container for a JSON array deserialized with Newtonsoft.Json.
Its constructor and backing field are both `internal`, so instances are only created through `JsonConvert.DeserializeObject<T>`.

```csharp
using Jeomseon.Data;
using Newtonsoft.Json;

var result = JsonConvert.DeserializeObject<JsonDataList<string>>(json);
IReadOnlyList<string> items = result.Data;
```

## Contract

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

## Refactoring Policy

Code that can be replaced with Unity's built-in equivalents is tracked via Korean TODO comments in the source and the Unreleased section of the CHANGELOG.
