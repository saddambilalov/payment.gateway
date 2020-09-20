using FluentValidation;

namespace Payment.Gateway.Api.Validators
{
    using Abstractions.Requests;

    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            this.RuleFor(x => x.Amount)
                .GreaterThan(0);
            
            this.RuleFor(x => x.Currency)
                .NotNull()
                .IsInEnum();

            this.RuleFor(x => x.CardDetails)
                .NotNull()
                .SetValidator(new CardDetailsValidator());
        }
    }
}
