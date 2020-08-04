using Dal.Repositories;
using Domain.Interfaces.Repositories;
using EfContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
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

        private Mock<ILogger<UnitOfWork>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<UnitOfWork>>();

            _dbContextOptionsBuilder = new DbContextOptionsBuilder();
            _dbContextOptionsBuilder.UseSqlServer("Initial Catalog=TrnsactionsDb;Data Source=.;Integrated Security=true;");


            
            _transactionContext = new TransactionContext(_dbContextOptionsBuilder.Options);
            _transactionRepository = new TransactionRepository(_transactionContext);

            _unitOfWork = new UnitOfWork(_transactionContext, _transactionRepository, _loggerMock.Object);

            _efContextTransaction = _transactionContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            if (_efContextTransaction != null)
            {
                _efContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task InsertList_Succeed()
        {
            var efContextTransCount = _unitOfWork.TransactionRepository.GetAll().Count;
            
            await  _transactionRepository.InsertListAsync(
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

            await _unitOfWork.SaveAsync();

            Assert.AreEqual(efContextTransCount + 1, _transactionRepository.GetAll().Count);
        }

        [Test]
        public async Task InsertList_Failed_ForgotToCallSaveChanges()
        {
            var efContextTransCount = _unitOfWork.TransactionRepository.GetAll().Count;

            await _transactionRepository.InsertListAsync(
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

        [Test]
        public async Task InsertList_Failed_PartOfTransactionsFailed()
        {
            _efContextTransaction.Rollback();
            _efContextTransaction = null;

            var efContextTransCount = _unitOfWork.TransactionRepository.GetAll().Count;

            await _transactionRepository.InsertListAsync(
                new List<Transaction> {
                    new Transaction
                    {
                        Amount = 0.01M,
                        Currency = "USD",
                        Date = DateTime.Now,
                        Status = 1,
                        TransactionId = "transaction001"
                    },
                    new Transaction
                    {
                        Amount = 0.01M,
                        Currency = null,
                        Date = DateTime.Now,
                        Status = 1,
                        TransactionId = "transaction002"
                    },
                });

            Assert.ThrowsAsync<DbUpdateException>(async () => await _unitOfWork.SaveWithTransactionAsync());

            Assert.AreEqual(efContextTransCount, _transactionRepository.GetAll().Count);
        }
    }
}