namespace Payment.Gateway.ApiTests.Services
{
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Api.Abstractions.Requests;
    using Api.Clients;
    using Api.Clients.Contracts;
    using Api.Services;
    using AutoFixture;
    using AutoMapper;
    using Domain.ValueObjects;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Refit;
    using Xunit;

    public class BankPaymentIssuerServiceTests
    {
        private readonly Mock<IBankClient> _bankClientMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly BankPaymentIssuerService _bankPaymentIssuerService;
        private readonly Fixture _fixture;

        public BankPaymentIssuerServiceTests()
        {
            _bankClientMock = new Mock<IBankClient>();
            _mapperMock = new Mock<IMapper>();

            _bankPaymentIssuerService = new BankPaymentIssuerService(
                _bankClientMock.Object, _mapperMock.Object, Mock.Of<ILogger<BankPaymentIssuerService>>()
                );

            _fixture = new Fixture();
        }

        [Fact]
        public async Task When_Bank_Issues_Payment_Then_Return_Success()
        {
            //arrange
            var paymentRequest = _fixture.Create<PaymentRequest>();
            var bankPaymentRequest = _fixture.Create<BankPaymentRequest>();
            var bankPaymentResponse = _fixture.Create<BankPaymentResponse>();
            var token = _fixture.Create<CancellationToken>();

            _mapperMock.Setup(_ =>
                _.Map<BankPaymentRequest>(paymentRequest))
                .Returns(bankPaymentRequest);

            _bankClientMock.Setup(_ =>
                _.ProcessPaymentAsync(bankPaymentRequest, token))
                .ReturnsAsync(bankPaymentResponse);

            //act
            var result = await _bankPaymentIssuerService.IssuePaymentAsync(paymentRequest, token);

            //assert
            result.Status.Should().Be(PaymentStatus.Success);
            result.TransactionId.Should().Be(bankPaymentResponse.Id);
        }

        [Fact]
        public async Task When_Bank_Could_Not_Issue_Payment_Then_Return_Failed()
        {
            //arrange
            var paymentRequest = _fixture.Create<PaymentRequest>();
            var bankPaymentRequest = _fixture.Create<BankPaymentRequest>();
            var token = _fixture.Create<CancellationToken>();
            var bankPaymentResponse = _fixture.Create<BankPaymentResponse>();

            _mapperMock.Setup(_ =>
                    _.Map<BankPaymentRequest>(paymentRequest))
                .Returns(bankPaymentRequest);

            var apiException = ApiException.Create(new HttpRequestMessage(),
                HttpMethod.Post, new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.PaymentRequired,
                    Content = new StringContent(JsonSerializer.Serialize(bankPaymentResponse))
                }, new RefitSettings
                {
                    ContentSerializer = new SystemTextJsonContentSerializer()
                }).Result;

            _bankClientMock.Setup(_ =>
                    _.ProcessPaymentAsync(bankPaymentRequest, token))
                .ThrowsAsync(apiException);

            //act
            var result = await _bankPaymentIssuerService.IssuePaymentAsync(paymentRequest, token);

            //assert
            result.Status.Should().Be(PaymentStatus.Failed);
            result.TransactionId.Should().Be(bankPaymentResponse.Id);
        }
    }
}