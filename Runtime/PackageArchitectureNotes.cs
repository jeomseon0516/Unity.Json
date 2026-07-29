namespace Jeomseon.Data
{
    internal static class PackageArchitectureNotes
    {
        // TODO(api): 단순 직렬화 모델은 Unity JsonUtility로 대체 가능한지 확인하고, 다형성·사전 지원이 필요한 경우에만 Newtonsoft.Json을 유지합니다.
        // TODO(lifecycle): 정적 이벤트와 전역 인스턴스는 Domain Reload 비활성화 환경에서 초기화 상태가 남는지 검증합니다.
    }
}
