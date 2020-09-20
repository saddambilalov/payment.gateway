namespace Payment.Gateway.ApiTests.Controllers
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Api.Abstractions.Requests;
    using Api.Abstractions.Responses;
    using Api.Services.Interfaces;
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Payment.Gateway.Api.Controllers;
    using Xunit;

    public class PaymentGatewayControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;

        private readonly PaymentGatewayController _paymentGatewayController;
        private readonly Fixture _fixture;

        public PaymentGatewayControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();

            _paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task When_No_Payment_Found_Then_Return_NotFound()
        {
            //arrange
            var transactionId = _fixture.Create<Guid>();
            var token = _fixture.Create<CancellationToken>();

            _paymentServiceMock.Setup(_ => _.GetPaymentDetailsWithTransactionIdAsync(
                    It.Is<Guid>(providedTransactionId => providedTransactionId == transactionId),
                    It.Is<CancellationToken>(providedToken => providedToken == token))).
                ReturnsAsync((PaymentResponse)null);

            //act
            var result = await _paymentGatewayController.GetAsync(transactionId, token);

            //assert
            result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_Payment_Found_Then_Return_Ok()
        {
            //arrange
            var transactionId = _fixture.Create<Guid>();
            var token = _fixture.Create<CancellationToken>();
            var expectedPaymentResponse = _fixture.Create<PaymentResponse>();

            _paymentServiceMock.Setup(_ => _.GetPaymentDetailsWithTransactionIdAsync(
                    It.Is<Guid>(providedTransactionId => providedTransactionId == transactionId),
                    It.Is<CancellationToken>(providedToken => providedToken == token))).
                ReturnsAsync(expectedPaymentResponse);

            //act
            var result = await _paymentGatewayController.GetAsync(transactionId, token);

            //assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedPaymentResponse);
        }


        [Fact]
        public async Task When_Payment_Proceed_Then_Return_Ok()
        {
            //arrange
            var token = _fixture.Create<CancellationToken>();
            var paymentRequest = _fixture.Create<PaymentRequest>();
            var expectedPaymentIssuedResponse = _fixture.Create<PaymentIssuedResponse>();

            _paymentServiceMock.Setup(_ => _.ProceedPaymentAsync(
                    It.Is<PaymentRequest>(providedPaymentRequest => providedPaymentRequest.Equals(paymentRequest)),
                    It.Is<CancellationToken>(providedToken => providedToken == token))).
                ReturnsAsync(expectedPaymentIssuedResponse);

            //act
            var result = await _paymentGatewayController.PostAsync(paymentRequest, token);

            //assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedPaymentIssuedResponse);
        }
    }
}