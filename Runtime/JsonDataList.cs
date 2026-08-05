using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Jeomseon.Data
{
    [Serializable]
    public sealed class JsonDataList<T>
    {
        [JsonProperty] internal List<T> DataList { get; set; }

        public IReadOnlyList<T> Data => DataList;
        /* TODO(P2-01, api): 단순 직렬화 모델은 Unity JsonUtility로 대체 가능한지 확인하고,
         * 다형성·사전 지원이 필요한 경우에만 Newtonsoft.Json 의존성을 유지합니다.
         */
        internal JsonDataList() {}
    }
}
