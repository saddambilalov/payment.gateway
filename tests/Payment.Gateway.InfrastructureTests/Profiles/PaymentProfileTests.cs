namespace Payment.Gateway.InfrastructureTests.Profiles
{
    using AutoFixture;
    using AutoMapper;
    using Domain.Entities;
    using Domain.ValueObjects;
    using FluentAssertions;
    using FluentAssertions.Common;
    using Infrastructure.DataPersistence.Models;
    using Infrastructure.Profiles;
    using Infrastructure.Services.Interfaces;
    using Moq;
    using Xunit;

    public class PaymentProfileTests
    {
        private readonly Mock<ICipherService> _cipherServiceMock;
        private readonly MapperConfiguration _mapperConfig;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public PaymentProfileTests()
        {
            _cipherServiceMock = new Mock<ICipherService>();

            _mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new PaymentProfile(_cipherServiceMock.Object));
            });

            _mapper = _mapperConfig.CreateMapper();

            _fixture = new Fixture();
        }

        [Fact]
        public void GivenMapperConfiguration_ThenItIsValid()
        {
            // assert
            _mapperConfig.AssertConfigurationIsValid();
        }

        [Fact]
        public void When_Mapping_Payment_To_PaymentModel_Should_Pass()
        {
            //arrange
            var payment = _fixture.Create<Payment>();

            var encryptedCardDetails = _fixture.Create<byte[]>();

            _cipherServiceMock.Setup(_ =>
                    _.Encrypt(It.Is<CardDetails>(providedCardDetails => providedCardDetails == payment.CardDetails)))
                .Returns(encryptedCardDetails);

            //act
            var paymentModel = _mapper.Map<PaymentModel>(payment);

            //assert
            paymentModel.Should().NotBeNull();
            paymentModel.Amount.Should().IsSameOrEqualTo(payment.Id);
            paymentModel.BankPaymentResult.Should().BeEquivalentTo(payment.BankPaymentResult);
            paymentModel.Currency.Should().BeEquivalentTo(payment.Currency);
            paymentModel.CardDetails.Should().BeEquivalentTo(encryptedCardDetails);
        }

        [Fact]
        public void When_Mapping_PaymentModel_To_Payment_Should_Pass()
        {
            //arrange
            var paymentModel = _fixture.Create<PaymentModel>();

            var decryptedCardDetails = _fixture.Create<CardDetails>();

            _cipherServiceMock.Setup(_ =>
                    _.Decrypt<CardDetails>(
                        It.Is<byte[]>(providedCardDetails => providedCardDetails.Equals(paymentModel.CardDetails))))
                .Returns(decryptedCardDetails);

            //act
            var payment = _mapper.Map<Payment>(paymentModel);

            //assert
            payment.Should().NotBeNull();
            payment.Amount.Should().IsSameOrEqualTo(paymentModel.Id);
            payment.BankPaymentResult.Should().BeEquivalentTo(paymentModel.BankPaymentResult);
            payment.Currency.Should().BeEquivalentTo(paymentModel.Currency);
            payment.CardDetails.Should().BeEquivalentTo(decryptedCardDetails);
        }
    }
}