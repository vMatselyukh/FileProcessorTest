using AutoMapper;
using Bll.Automapper;
using Domain.Enums;
using Domain.Models;
using NUnit.Framework;
using System;

namespace BllTests
{
    public class TransactionMappingsTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var transactionProfile = new TransactionProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(transactionProfile));
            _mapper = new Mapper(configuration);
        }

        [Test]
        public void TransactionTransactionEfBackAndForth_Succeed()
        {
            var transactionDate = DateTime.Now;

            var commonTransaction = new Transaction
            {
                Amount = 450.50M,
                Currency = "UAH",
                Date = transactionDate,
                Status = TransactionStatusEnum.D,
                TransactionId = "transaction01"
            };

            var efTransaction = new EfContext.Transaction
            {
                Amount = 450.50M,
                Currency = "UAH",
                Date = transactionDate,
                Status = 3,
                TransactionId = "transaction01"
            };

            var mappedEfTransaction = _mapper.Map<EfContext.Transaction>(commonTransaction);

            Assert.AreEqual(efTransaction.Amount, mappedEfTransaction.Amount);
            Assert.AreEqual(efTransaction.Currency, mappedEfTransaction.Currency);
            Assert.AreEqual(efTransaction.Date, mappedEfTransaction.Date);
            Assert.AreEqual(efTransaction.Status, mappedEfTransaction.Status);
            Assert.AreEqual(efTransaction.TransactionId, mappedEfTransaction.TransactionId);

            var mappedCommonTransaction = _mapper.Map<Transaction>(efTransaction);

            Assert.AreEqual(commonTransaction.Amount, mappedCommonTransaction.Amount);
            Assert.AreEqual(commonTransaction.Currency, mappedCommonTransaction.Currency);
            Assert.AreEqual(commonTransaction.Date, mappedCommonTransaction.Date);
            Assert.AreEqual(commonTransaction.Status, mappedCommonTransaction.Status);
            Assert.AreEqual(commonTransaction.TransactionId, mappedCommonTransaction.TransactionId);
        }
    }
}
