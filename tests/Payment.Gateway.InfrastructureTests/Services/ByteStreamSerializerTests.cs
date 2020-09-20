namespace Payment.Gateway.InfrastructureTests.Services
{
    using System;
    using AutoFixture;
    using Fixture;
    using FluentAssertions;
    using Infrastructure.Services;
    using Xunit;

    public class ByteStreamSerializerTests
    {
        private readonly ByteStreamSerializer _serializer;
        private readonly Fixture _fixture;

        public ByteStreamSerializerTests()
        {
            _serializer = new ByteStreamSerializer();
            _fixture = new Fixture();
        }

        [Fact()]
        public void Validate_Serialization_With_Deserialization_For_Serializable_Object()
        {
            //arrange
            var sampleObject = _fixture.Create<SerializableObject>();

            //act
            var serializedObject = _serializer.Serialize(sampleObject);
            var deserializedObject = _serializer.Deserialize<SerializableObject>(serializedObject);

            //assert
            deserializedObject.Should().BeEquivalentTo(sampleObject);
        }


        [Fact()]
        public void Validate_Serialization_With_Deserialization_For_Primitive_Type()
        {
            //arrange
            var primitiveType = _fixture.Create<int>();

            //act
            var serializedObject = _serializer.Serialize(primitiveType);
            var deserializedObject = _serializer.Deserialize<int>(serializedObject);

            //assert
            deserializedObject.Should().Be(primitiveType);
        }

        [Fact()]
        public void Validate_When_Object_Is_Not_Serializable_Then_Throw_Exception()
        {
            //arrange
            var sampleObject = _fixture.Create<NoneSerializableObject>();

            //act
            Action trySerialize = () => _serializer.Serialize(sampleObject);

            //assert
            trySerialize.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}