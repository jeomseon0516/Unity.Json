# Json 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## 작업 순서

1. **P1-01 — 직렬화 계약 명확화**
   - null, 빈 목록, 중첩 타입, Unity 객체 및 버전이 다른 데이터의 동작을 테스트합니다.
2. **P2-01 — JsonUtility와 Newtonsoft 역할 구분**
   - 단순 모델은 Unity 기본 기능으로 대체하고 다형성·Dictionary 등 필요한 기능만 Newtonsoft에 둡니다.
3. **P2-02 — 공개 모델 API 개선**
   - 내부 setter와 생성자 정책, 불변 조회 API 및 오류 결과를 문서화합니다.
4. **P3-01 — Serializer 추상화**
   - 여러 구현이 실제로 필요해질 때만 선택 가능한 serializer 계약을 도입합니다.
