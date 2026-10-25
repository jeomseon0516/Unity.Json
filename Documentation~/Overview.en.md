# Jeomseon Unity JSON

[한국어](Overview.md) · [Installation](../README.en.md)

## Purpose

A small container exposing the DataList JSON property as IReadOnlyList after Newtonsoft.Json deserialization.

## Usage flow

Call JsonConvert.DeserializeObject<JsonDataList<T>>(json), inspect Data, and consume the read-only list. Its constructor and DataList setter are internal.

## Contracts and limitations

Missing/null DataList gives Data==null; [] gives Count==0. Read-only list access does not make element objects immutable. Handle Newtonsoft exceptions for invalid JSON/type mismatches.

## Package requirements

- Package ID: `com.jeomseon.unity.json`
- Version: `0.3.1`
- Unity: `6000.6` / `0f1`
- `com.unity.nuget.newtonsoft-json`: `3.2.2`

## Samples and verification

Import the following from Package Manager > Samples. Open the included Scene or Editor tool; follow any Setup step in its sample README.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [JsonSample](../Samples~/BasicUsage/JsonSample.unity)

[Test sources](../Tests)

Add the package ID to manifest `testables`, then run the relevant Unity Test Runner assemblies. These links describe available tests, not a new successful test run.

## Source map

- [JsonDataList](../Runtime/JsonDataList.cs)
