namespace Payment.Gateway.Api.Profiles
{
    using Abstractions.Requests;
    using Abstractions.Resources;
    using Abstractions.Responses;
    using AutoMapper;
    using Clients.Contracts;
    using Domain.Entities;
    using Domain.ValueObjects;

    public class PaymentResourceProfile : Profile
    {
        public PaymentResourceProfile()
        {
            CreateMap<PaymentRequest, Payment>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.BankPaymentResult,
                    opt => opt.Ignore());

            CreateMap<PaymentRequest, BankPaymentRequest>();

            CreateMap<BankPaymentResult, PaymentIssuedResponse>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(
                        src => src.Status.ToString()));

            CreateMap<CardDetails, CardDetailsResource>()
                .ForMember(dest => dest.CardNumber,
                    opt => opt.MapFrom(
                        src => $"xxxx-xxxx-xxxx-{src.CardNumber.Substring(src.CardNumber.Length - 4, 4)}"));

            CreateMap<Payment, PaymentResponse>()
                .ForMember(dest => dest.TransactionId,
                    opt => opt.MapFrom(src => src.BankPaymentResult.TransactionId))
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.BankPaymentResult.Status));
        }
    }
}
