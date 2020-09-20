namespace Payment.Gateway.ApiTests.Profiles
{
    using Api.Abstractions.Requests;
    using Api.Abstractions.Resources;
    using Api.Profiles;
    using AutoFixture;
    using AutoMapper;
    using Domain.Entities;
    using Domain.ValueObjects;
    using FluentAssertions;
    using FluentAssertions.Common;
    using Xunit;

    public class PaymentResourceProfileTests
    {
        private readonly MapperConfiguration _mapperConfig;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public PaymentResourceProfileTests()
        {
            _mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<PaymentResourceProfile>();
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
        public void When_Mapping_PaymentRequest_To_Payment_Should_Pass()
        {
            //arrange
            var paymentRequest = _fixture.Create<PaymentRequest>();

            //act
            var payment = _mapper.Map<Payment>(paymentRequest);

            //assert
            payment.Should().NotBeNull();
            payment.Amount.Should().IsSameOrEqualTo(paymentRequest.Amount);
            payment.Currency.Should().BeEquivalentTo(paymentRequest.Currency);
            payment.Merchant.Should().BeEquivalentTo(paymentRequest.Merchant);
            payment.CardDetails.Should().BeEquivalentTo(paymentRequest.CardDetails);
        }

        [Fact]
        public void When_Mapping_CardDetails_To_CardDetailsResource_Should_Pass()
        {
            //arrange
            var cardDetails = _fixture.Create<CardDetails>();

            //act
            var cardDetailsResource = _mapper.Map<CardDetailsResource>(cardDetails);

            //assert
            cardDetailsResource.Should().NotBeNull();
            cardDetailsResource.CardExpiryMonth.Should().Be(cardDetails.CardExpiryMonth);
            cardDetailsResource.CardExpiryYear.Should().Be(cardDetails.CardExpiryYear);
            cardDetailsResource.CardNumber.Should().StartWith("xxxx-xxxx-xxxx-");
            cardDetailsResource.CardNumber.Should().EndWith(cardDetails.CardNumber.Substring(cardDetails.CardNumber.Length - 4, 4));
        }
    }
}