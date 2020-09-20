namespace Payment.Gateway.Infrastructure.Profiles
{
    using AutoMapper;
    using DataPersistence.Models;
    using Domain.Entities;
    using Domain.ValueObjects;
    using Services.Interfaces;

    public class PaymentProfile : Profile
    {
        public PaymentProfile(ICipherService cipherService)
        {
            CreateMap<Payment, PaymentModel>()
                 .ForMember(
                    dest => dest.CardDetails, opt => opt.MapFrom(src => cipherService.Encrypt(src.CardDetails)))
                .DisableCtorValidation();

            CreateMap<PaymentModel, Payment>()
                .ForMember(
                    dest => dest.Merchant, opt => opt.Ignore())
                .ForMember(
                    dest => dest.CardDetails, opt => opt.MapFrom(src => cipherService.Decrypt<CardDetails>(src.CardDetails)))
                .DisableCtorValidation();
        }
    }
}
