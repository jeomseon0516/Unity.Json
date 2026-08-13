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
