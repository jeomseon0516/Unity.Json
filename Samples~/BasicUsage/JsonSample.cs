using Jeomseon.Data;
using UnityEngine;

namespace Jeomseon.Samples.Json
{
    public sealed class JsonSample : MonoBehaviour
    {
        [ContextMenu("지원 타입 확인")]
        private void PrintSupportedType()
        {
            Debug.Log($"JSON 목록 래퍼: {typeof(JsonDataList<string>).FullName}");
        }
    }
}
