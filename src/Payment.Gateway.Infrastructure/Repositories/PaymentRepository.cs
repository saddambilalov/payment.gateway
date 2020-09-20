namespace Payment.Gateway.Infrastructure.Repositories
{
    using MongoDB.Driver;
    using Payment.Gateway.Domain.Repositories;
    using System.Threading;
    using System.Threading.Tasks;
    using Payment.Gateway.Domain.Entities;
    using System;
    using DataPersistence.Configuration;
    using DataPersistence.Models;
    using AutoMapper;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<PaymentModel> _paymentCollection;
        private readonly IMapper _mapper;

        public PaymentRepository(PaymentGatewayDbSettings paymentGatewayDbSettings, 
            IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(paymentGatewayDbSettings.ConnectionString);
            var database = client.GetDatabase(paymentGatewayDbSettings.DatabaseName);

            _paymentCollection = database.GetCollection<PaymentModel>(
                paymentGatewayDbSettings.CollectionName);
        }

        public async Task AddAsync(Payment payment, CancellationToken token)
        {
            var paymentModel = _mapper.Map<PaymentModel>(payment);
            await _paymentCollection.InsertOneAsync(paymentModel, new InsertOneOptions(), token);
        }

        public async Task<Payment> FindByTransactionIdAsync(Guid transactionId, CancellationToken token)
        {
            var filter = Builders<PaymentModel>.Filter.Where(_ => 
                _.BankPaymentResult.TransactionId == transactionId);

            var findOptions = new FindOptions<PaymentModel> { Limit = 1 };


            var paymentResult = await _paymentCollection.FindAsync(filter, findOptions, token);

            return _mapper.Map<Payment>(paymentResult.FirstOrDefault());
        }
    }
}