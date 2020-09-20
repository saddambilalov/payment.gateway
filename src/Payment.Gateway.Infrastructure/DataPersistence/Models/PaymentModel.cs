using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Infrastructure.DataPersistence.Models
{
    using Domain.Entities;
    using Domain.ValueObjects;

    public class PaymentModel
    { 
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public Currency Currency { get; set; }

        public byte[] CardDetails { get; set; }

        public BankPaymentResult BankPaymentResult { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
