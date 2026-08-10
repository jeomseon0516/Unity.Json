# JSON 기본 예제

`JsonSample.unity` Scene을 열면 `JsonSample` 컴포넌트가 이미 붙은 "Json Sample" GameObject가
구성되어 있습니다. Inspector의 컨텍스트 메뉴(⋮)에서 각 항목을 실행해 `JsonDataList<T>`의
직렬화 계약을 Console 로그로 확인합니다.

1. **정상 역직렬화**: `["a","b"]`를 역직렬화해 `Data.Count`와 원소 값이 그대로 나오는지 확인합니다.
2. **null 목록과 빈 목록 구분**: `DataList` 키가 없는 JSON은 `Data == null`, 빈 배열(`[]`)은
   `Data == null`이 아니면서 `Count == 0`으로 서로 다르게 처리되는지 확인합니다.
3. **오류 데이터: 타입 불일치**: 원소 타입이 맞지 않는 JSON을 역직렬화하면 예외 없이 조용히
   깨지지 않고 `JsonReaderException`이 발생하는지 확인합니다.
4. **지원 타입 확인**: 패키지의 JSON 목록 래퍼 타입이 정상적으로 참조되는지 확인합니다.
