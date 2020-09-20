using Microsoft.AspNetCore.Mvc;

namespace Payment.Gateway.Api.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using Abstractions.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Services.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentGatewayController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(PaymentIssuedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            var bankPaymentResult = await _paymentService.ProceedPaymentAsync(paymentRequest, cancellationToken);

            return Ok(bankPaymentResult);
        }

        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var paymentResult =
                await _paymentService.GetPaymentDetailsWithTransactionIdAsync(transactionId, cancellationToken);

            if (paymentResult == null)
            {
                return NotFound(new
                {
                    transactionId
                });
            }

            return Ok(paymentResult);
        }
    }
}
