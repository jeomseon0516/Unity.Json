using System.Collections.Generic;
using Jeomseon.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Jeomseon.Tests
{
    public sealed class JsonDataListSerializationTests
    {
        private sealed class NestedModel
        {
            public string Name { get; set; }
            public List<int> Values { get; set; }
            public Dictionary<string, string> Meta { get; set; }
        }

        [Test]
        public void Data_IsNull_WhenNeverAssigned()
        {
            var target = new JsonDataList<string>();

            Assert.That(target.Data, Is.Null);
        }

        [Test]
        public void Deserialize_MissingDataListKey_ResultsInNullData()
        {
            var target = JsonConvert.DeserializeObject<JsonDataList<string>>("{}");

            Assert.That(target.Data, Is.Null);
        }

        [Test]
        public void Deserialize_ExplicitNullDataList_ResultsInNullData()
        {
            var target = JsonConvert.DeserializeObject<JsonDataList<string>>("{\"DataList\":null}");

            Assert.That(target.Data, Is.Null);
        }

        [Test]
        public void Deserialize_EmptyArray_ResultsInEmptyNotNullData()
        {
            var target = JsonConvert.DeserializeObject<JsonDataList<string>>("{\"DataList\":[]}");

            Assert.That(target.Data, Is.Not.Null);
            Assert.That(target.Data, Is.Empty);
        }

        [Test]
        public void Serialize_EmptyList_ProducesEmptyArrayNotNull()
        {
            var target = new JsonDataList<string> { DataList = new List<string>() };

            string json = JsonConvert.SerializeObject(target);

            Assert.That(json, Is.EqualTo("{\"DataList\":[]}"));
        }

        [Test]
        public void Serialize_DoesNotIncludeReadOnlyDataProperty()
        {
            // .. Data는 DataList의 파생 View이며 별도 직렬화 대상이 아니다.
            // JsonIgnore가 없으면 Newtonsoft가 읽기 전용 공개 프로퍼티까지 직렬화해
            // 동일한 값이 "DataList"와 "Data" 두 키로 중복 노출된다.
            var target = new JsonDataList<string> { DataList = new List<string> { "a" } };

            string json = JsonConvert.SerializeObject(target);

            Assert.That(json, Does.Not.Contain("\"Data\":"));
        }

        [Test]
        public void RoundTrip_NestedComplexType_PreservesListsAndDictionaries()
        {
            const string json =
                "{\"DataList\":[{\"Name\":\"a\",\"Values\":[1,2,3],\"Meta\":{\"k\":\"v\"}}]}";

            var target = JsonConvert.DeserializeObject<JsonDataList<NestedModel>>(json);

            Assert.That(target.Data.Count, Is.EqualTo(1));
            Assert.That(target.Data[0].Name, Is.EqualTo("a"));
            Assert.That(target.Data[0].Values, Is.EqualTo(new List<int> { 1, 2, 3 }));
            Assert.That(target.Data[0].Meta["k"], Is.EqualTo("v"));
        }

        [Test]
        public void Deserialize_UnityValueType_PopulatesPublicFields()
        {
            // .. Vector3는 프로퍼티가 아닌 public 필드(x, y, z)로 값을 노출하는
            // 대표적인 Unity 값 타입이다. 역직렬화 시 필드가 정상적으로 채워지는지 확인한다.
            // JsonDataList<T>의 실사용 방향은 "외부 JSON을 받아 읽는" 역직렬화이므로 이 방향만
            // 계약으로 보장한다(아래 테스트 참고: 직렬화 방향은 별도 제약이 있다).
            var target = JsonConvert.DeserializeObject<JsonDataList<Vector3>>(
                "{\"DataList\":[{\"x\":1.0,\"y\":2.0,\"z\":3.0}]}");

            Assert.That(target.Data[0], Is.EqualTo(new Vector3(1f, 2f, 3f)));
        }

        [Test]
        public void Serialize_UnityValueTypeWithSelfReferentialProperty_Throws()
        {
            // .. Vector3.normalized는 Vector3를 반환하고 그 결과의 normalized도 다시 Vector3를
            // 반환해 타입 구조상 종료되지 않는다. Newtonsoft의 기본 순환 참조 감지가 이를 무한
            // 루프로 판단해 JsonSerializationException을 던진다(실측 확인, 2026-08-10). 따라서
            // Vector3처럼 자기 참조형 프로퍼티를 가진 타입은 이 컨테이너로 직렬화(쓰기)하는
            // 용도로 사용할 수 없고, 역직렬화(읽기) 전용으로만 사용해야 한다.
            var target = new JsonDataList<Vector3> { DataList = new List<Vector3> { new Vector3(1f, 2f, 3f) } };

            Assert.Throws<JsonSerializationException>(() => JsonConvert.SerializeObject(target));
        }

        [Test]
        public void Deserialize_UnknownTopLevelField_IsIgnored()
        {
            // .. 스키마에 없는 필드가 섞인 신버전(또는 다른 생산자) 데이터도
            // 예외 없이 알려진 필드만 채워 넣어야 한다.
            const string json = "{\"DataList\":[\"a\",\"b\"],\"SchemaVersion\":2,\"FutureField\":\"unknown\"}";

            var target = JsonConvert.DeserializeObject<JsonDataList<string>>(json);

            Assert.That(target.Data, Is.EqualTo(new List<string> { "a", "b" }));
        }

        [Test]
        public void Deserialize_TypeMismatchedElement_ThrowsInsteadOfSilentlyCorrupting()
        {
            Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<JsonDataList<int>>("{\"DataList\":[1,\"not-a-number\",3]}"));
        }

        [Test]
        public void Deserialize_MalformedJson_Throws()
        {
            Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<JsonDataList<string>>("not json at all"));
        }
    }
}
