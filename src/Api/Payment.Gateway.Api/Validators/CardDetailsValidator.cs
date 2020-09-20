namespace Payment.Gateway.Api.Validators
{
    using System;
    using Domain.ValueObjects;
    using FluentValidation;

    public class CardDetailsValidator : AbstractValidator<CardDetails>
    {
        public CardDetailsValidator()
        {
            this.RuleFor(x => x.CardNumber)
                .NotEmpty()
                .CreditCard();

            this.RuleFor(x => x.Cvv)
                .InclusiveBetween(100, 9999);

            this.RuleFor(x => x.CardExpiryMonth)
                .InclusiveBetween(1, 12);

            this.RuleFor(x => x.CardExpiryYear)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year);
        }
    }
}