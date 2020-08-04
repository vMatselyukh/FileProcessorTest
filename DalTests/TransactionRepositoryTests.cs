using Dal.Repositories;
using Domain.Interfaces.Repositories;
using EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DalTests
{
    public class TransactionRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private ITransactionRepository _transactionRepository;
        private TransactionContext _transactionContext;
        private DbContextOptionsBuilder _dbContextOptionsBuilder;
        private IDbContextTransaction _efContextTransaction;

        [SetUp]
        public void Setup()
        {
            _dbContextOptionsBuilder = new DbContextOptionsBuilder();
            _dbContextOptionsBuilder.UseSqlServer("Initial Catalog=TrnsactionsDb;Data Source=.;Integrated Security=true;");


            
            _transactionContext = new TransactionContext(_dbContextOptionsBuilder.Options);
            _transactionRepository = new TransactionRepository(_transactionContext);

            _unitOfWork = new UnitOfWork(_transactionContext, _transactionRepository);

            _efContextTransaction = _transactionContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            _efContextTransaction.Rollback();
        }

        [Test]
        public async Task InsertListSucceed()
        {
            var efContextTransCount = _unitOfWork.TransactionRepository.GetAll().Count;
            
            await  _transactionRepository.InsertList(
                new List<Transaction> { 
                    new Transaction
                    { 
                        Amount = 0.01M,
                        Currency = "USD",
                        Date = DateTime.Now,
                        Status = 1,
                        TransactionId = "transaction001"
                    }
                });

            await _unitOfWork.Save();

            Assert.AreEqual(efContextTransCount + 1, _transactionRepository.GetAll().Count);
        }

        [Test]
        public async Task InsertListFailed_ForgotToCallSaveChanges()
        {
            var efContextTransCount = _unitOfWork.TransactionRepository.GetAll().Count;

            await _transactionRepository.InsertList(
                new List<Transaction> {
                    new Transaction
                    {
                        Amount = 0.01M,
                        Currency = "USD",
                        Date = DateTime.Now,
                        Status = 1,
                        TransactionId = "transaction001"
                    }
                });

            Assert.AreEqual(efContextTransCount, _transactionRepository.GetAll().Count);
        }
    }
}