namespace Payment.Gateway.Api.Services
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using AutoMapper;
    using Clients;
    using Clients.Contracts;
    using Domain.Entities;
    using Domain.ValueObjects;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Refit;

    public class BankPaymentIssuerService : IBankPaymentIssuerService
    {
        private readonly IBankClient _bankClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BankPaymentIssuerService> _logger;

        public BankPaymentIssuerService(IBankClient bankClient, IMapper mapper, ILogger<BankPaymentIssuerService> logger)
        {
            _bankClient = bankClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BankPaymentResult> IssuePaymentAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            try
            {
                var bakPaymentRequest = _mapper.Map<BankPaymentRequest>(paymentRequest);
                var bankPaymentResponse = await _bankClient.ProcessPaymentAsync(bakPaymentRequest, cancellationToken);

                return new BankPaymentResult
                {
                    TransactionId = bankPaymentResponse.Id,
                    Status = PaymentStatus.Success
                };
            }
            catch (ApiException e) when (e.StatusCode >= HttpStatusCode.BadRequest &&
                                         e.StatusCode < HttpStatusCode.InternalServerError)
            {
                _logger.LogWarning(e.Message, e.Content);

                var bankPaymentResponse = await e.GetContentAsAsync<BankPaymentResponse>();

                return new BankPaymentResult
                {
                    TransactionId = bankPaymentResponse.Id,
                    Status = PaymentStatus.Failed
                };
            }
        }
    }
}