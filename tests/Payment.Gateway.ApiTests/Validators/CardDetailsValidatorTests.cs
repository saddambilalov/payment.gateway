namespace Payment.Gateway.ApiTests.Validators
{
    using System;
    using System.Collections.Generic;
    using Api.Validators;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CardDetailsValidatorTests
    {
        private readonly CardDetailsValidator _cardDetailsValidator;

        public CardDetailsValidatorTests()
        {
            _cardDetailsValidator = new CardDetailsValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("111111")]
        [InlineData("411111111111111")]
        public void When_CreditCardNumber_Is_Not_Valid_Then_Fail(string creditCardNumber)
        {
            _cardDetailsValidator
                .ShouldHaveValidationErrorFor(x => x.CardNumber, creditCardNumber);
        }

        [Theory]
        [InlineData("4111111111111111")]
        [InlineData("5500000000000004")]
        [InlineData("340000000000009")]
        public void When_CreditCardNumber_Is_Valid_Then_Pass(string creditCardNumber)
        {
            _cardDetailsValidator.ShouldNotHaveValidationErrorFor(x => x.CardNumber, creditCardNumber);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(222222)]
        [InlineData(33333)]
        public void When_Cvv_Is_Not_Valid_Then_Fail(int cvv)
        {
            _cardDetailsValidator
                .ShouldHaveValidationErrorFor(x => x.Cvv, cvv);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(9999)]
        public void When_Cvv_Is_Valid_Then_Pass(int cvv)
        {
            _cardDetailsValidator.ShouldNotHaveValidationErrorFor(x => x.Cvv, cvv);
        }

        [Theory]
        [InlineData(13)]
        [InlineData(-1)]
        public void When_CardExpiryMonth_Is_Not_Valid_Then_Fail(int cardExpiryMonth)
        {
            _cardDetailsValidator
                .ShouldHaveValidationErrorFor(x => x.CardExpiryMonth, cardExpiryMonth);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(9)]
        public void When_CardExpiryMonth_Is_Valid_Then_Pass(int cardExpiryMonth)
        {
            _cardDetailsValidator.ShouldNotHaveValidationErrorFor(x => x.CardExpiryMonth, cardExpiryMonth);
        }

        [Theory]
        [InlineData(2013)]
        [InlineData(2015)]
        public void When_CardExpiryYear_Is_Not_Valid_Then_Fail(int cardExpiryYear)
        {
            _cardDetailsValidator
                .ShouldHaveValidationErrorFor(x => x.CardExpiryYear, cardExpiryYear);
        }

        [Theory]
        [MemberData(nameof(ValidYears))]
        public void When_CardExpiryYear_Is_Valid_Then_Pass(int cardExpiryYear)
        {
            _cardDetailsValidator.ShouldNotHaveValidationErrorFor(x => x.CardExpiryYear, cardExpiryYear);
        }

        public static IEnumerable<object[]> ValidYears =>
            new List<object[]>
            {
                new object[] { DateTime.UtcNow.Year },
                new object[] { DateTime.UtcNow.AddYears(1).Year },
                new object[] { DateTime.UtcNow.AddYears(2).Year }
            };
    }
}