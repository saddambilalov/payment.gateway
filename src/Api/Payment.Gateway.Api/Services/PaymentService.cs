namespace Payment.Gateway.Api.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using Abstractions.Responses;
    using AutoMapper;
    using Domain.Entities;
    using Domain.Repositories;
    using Interfaces;

    public class PaymentService : IPaymentService
    {
        private readonly IBankPaymentIssuerService _bankPaymentIssuerService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IBankPaymentIssuerService bankPaymentIssuerService, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _bankPaymentIssuerService = bankPaymentIssuerService;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentIssuedResponse> ProceedPaymentAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken)
        {
            var bankPaymentResult = await _bankPaymentIssuerService.IssuePaymentAsync(paymentRequest, cancellationToken);

            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.BankPaymentResult = bankPaymentResult;
            await _paymentRepository.AddAsync(payment, cancellationToken);

            return _mapper.Map<PaymentIssuedResponse>(bankPaymentResult);
        }

        public async Task<PaymentResponse> GetPaymentDetailsWithTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var paymentDetails = await _paymentRepository.FindByTransactionIdAsync(transactionId, cancellationToken);
            var paymentResult = _mapper.Map<PaymentResponse>(paymentDetails);
            return paymentResult;
        }
    }
}