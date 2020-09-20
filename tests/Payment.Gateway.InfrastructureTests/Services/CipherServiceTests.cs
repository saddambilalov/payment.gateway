namespace Payment.Gateway.InfrastructureTests.Services
{
    using System;
    using AutoFixture;
    using Fixture;
    using FluentAssertions;
    using Infrastructure.Services;
    using Infrastructure.Services.Settings;
    using Microsoft.AspNetCore.DataProtection;
    using Xunit;

    public class CipherServiceTests
    {
        private readonly CipherService _cipherService;
        private readonly Fixture _fixture;

        public CipherServiceTests()
        {
            var options = new CipherSettings
            {
                Key = Guid.NewGuid().ToString()
            };
            _cipherService = new CipherService(new ByteStreamSerializer(), new EphemeralDataProtectionProvider(), options);

            _fixture = new Fixture();
        }


        [Fact()]
        public void Validate_Encryption_With_Decryption_For_Serializable_Object()
        {
            //arrange
            var sampleObject = _fixture.Create<SerializableObject>();

            //act
            var serializedObject = _cipherService.Encrypt(sampleObject);
            var deserializedObject = _cipherService.Decrypt<SerializableObject>(serializedObject);

            //assert
            deserializedObject.Should().BeEquivalentTo(sampleObject);
        }
    }
}