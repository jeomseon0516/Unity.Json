using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Jeomseon.Data
{
    /// <summary>
    /// Newtonsoft.Json으로 역직렬화한 JSON 배열을 담는 읽기 전용 컨테이너입니다.
    /// 생성자와 <see cref="DataList"/> setter가 모두 internal인 이유는 이 타입의 유일한 생성 경로를
    /// <see cref="JsonConvert.DeserializeObject{T}(string)"/>로 한정해, 역직렬화 이후 외부 코드가
    /// 내용을 직접 구성하거나 변형하지 못하게 하기 위함입니다.
    /// </summary>
    /// <typeparam name="T">
    /// 목록 원소 타입입니다. Dictionary·중첩 클래스 등 Newtonsoft.Json이 지원하는 임의의 타입을 사용할 수 있습니다.
    /// </typeparam>
    [Serializable]
    public sealed class JsonDataList<T>
    {
        [JsonProperty] internal List<T> DataList { get; set; }

        /// <summary>
        /// 역직렬화된 목록의 읽기 전용 뷰입니다.
        /// 원본 JSON에 <c>DataList</c> 키가 없거나 값이 명시적으로 <c>null</c>이면 이 프로퍼티도 <c>null</c>입니다.
        /// 빈 배열(<c>[]</c>)은 <c>null</c>이 아닌 빈 컬렉션(<c>Count == 0</c>)으로 구분됩니다.
        /// </summary>
        /// <remarks>
        /// 잘못된 JSON이거나 원소 타입이 <typeparamref name="T"/>와 맞지 않으면 역직렬화 시점에
        /// <see cref="Newtonsoft.Json.JsonReaderException"/>이 발생하며, 부분적으로 채워진 인스턴스는
        /// 반환되지 않습니다. 스키마에 없는 알 수 없는 필드는 조용히 무시됩니다.
        /// </remarks>
        [JsonIgnore] public IReadOnlyList<T> Data => DataList;
        /* P2-01 결정: DataList는 auto-property라 JsonUtility(필드만 직렬화)로 대체할 수 없고,
         * T에 Dictionary를 포함한 중첩 모델을 담는 사용을 지원해야 하므로 Newtonsoft.Json을 유지한다.
         */
        internal JsonDataList() {}
    }
}
