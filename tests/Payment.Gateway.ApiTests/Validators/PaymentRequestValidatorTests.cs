namespace Payment.Gateway.ApiTests.Validators
{
    using Api.Validators;
    using Domain.ValueObjects;
    using FluentValidation.TestHelper;
    using Xunit;

    public class PaymentRequestValidatorTests
    {
        private readonly PaymentRequestValidator _paymentRequestValidator;

        public PaymentRequestValidatorTests()
        {
            _paymentRequestValidator = new PaymentRequestValidator();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void When_Amount_Is_Not_Valid_Then_Fail(double amount)
        {
            _paymentRequestValidator
                .ShouldHaveValidationErrorFor(x => x.Amount, amount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0.4)]
        public void When_Amount_Is_Valid_Then_Pass(double amount)
        {
            _paymentRequestValidator.ShouldNotHaveValidationErrorFor(x => x.Amount, amount);
        }

        [Theory]
        [InlineData(Currency.Euro)]
        [InlineData(Currency.Usd)]
        public void When_Currency_Is_Valid_Then_Pass(Currency currency)
        {
            _paymentRequestValidator.ShouldNotHaveValidationErrorFor(x => x.Currency, currency);
        }
    }
}