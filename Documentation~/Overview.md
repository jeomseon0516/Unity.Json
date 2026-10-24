# Jeomseon Unity JSON

[English](Overview.en.md) · [설치](../README.md)

## 목적

Newtonsoft.Json으로 읽은 DataList JSON 프로퍼티를 IReadOnlyList로 노출하는 작은 컨테이너입니다.

## 사용 흐름

JsonConvert.DeserializeObject<JsonDataList<T>>(json) 호출 → 결과 Data 확인 → 읽기 전용 목록 소비. 생성자와 DataList setter는 internal입니다.

## 계약과 제한

JSON의 DataList 누락/null은 Data==null이고 []는 Count==0입니다. 읽기 전용 목록이 원소 객체의 불변성까지 보장하지 않습니다. 잘못된 JSON과 타입 불일치는 Newtonsoft 역직렬화 예외로 처리합니다.

## 패키지 요구 사항

- Package ID: `com.jeomseon.unity.json`
- Version: `0.3.1`
- Unity: `6000.6` / `0f1`
- `com.unity.nuget.newtonsoft-json`: `3.2.2`

## 샘플과 검증

Package Manager > Samples에서 다음 예제를 Import하세요. 포함된 Scene 또는 Editor 도구를 열고 샘플 README의 Setup 절차를 먼저 수행합니다.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [JsonSample](../Samples~/BasicUsage/JsonSample.unity)

[테스트 코드](../Tests)

소비 프로젝트 manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 어셈블리를 실행합니다. 위 링크는 테스트 위치이며 이번 문서 수정으로 새 테스트 실행을 주장하지 않습니다.

## 코드 지도

- [JsonDataList](../Runtime/JsonDataList.cs)
