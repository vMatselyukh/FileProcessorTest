using AutoMapper;
using Bll.Helpers;
using Bll.Managers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Models.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BllTests
{
    public class FileManagerTests
    {
        private IFileManager _fileManager;

        private Mock<ILogger<FileManager>> _loggerMock;
        private Mock<IFileParserFactory> _fileParserFactoryMock;
        private Mock<IFileParser> _csvFileParserMock;
        private Mock<ITransactionValidator> _transactionValidatorMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IMapper> _automapperMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<FileManager>>();
            _fileParserFactoryMock = new Mock<IFileParserFactory>();
            _transactionValidatorMock = new Mock<ITransactionValidator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _automapperMock = new Mock<IMapper>();
            _csvFileParserMock = new Mock<IFileParser>();

            _fileManager = new FileManager(_fileParserFactoryMock.Object, _loggerMock.Object,
                _transactionValidatorMock.Object, _unitOfWorkMock.Object, _automapperMock.Object);
        }

        [Test]
        public async Task ProcessFile_Succeed()
        {
            _fileParserFactoryMock.Setup(factory => factory.GetParser(".csv"))
                .Returns(_csvFileParserMock.Object);

            var fakeFileParseResult = new FileParseResult
            {
                IsSucceed = true,
                TransactionList = new List<Transaction>
                {
                    new Transaction
                    {
                        Amount = 1000M,
                        Currency = "USD",
                        Date = DateTime.Parse("20/02/2019 12:33:16"),
                        Status = TransactionStatusEnum.A,
                        TransactionId = "Invoice0000001"
                    },
                    new Transaction
                    {
                        Amount = 300M,
                        Currency = "USD",
                        Date = DateTime.Parse("21/02/2019 02:04:59"),
                        Status = TransactionStatusEnum.R,
                        TransactionId = "Invoice0000002"
                    },
                }
            };

            var fakeEfContextTransactions = new List<EfContext.Transaction>
            {
                new EfContext.Transaction
                {
                    Amount = 1000M,
                    Currency = "USD",
                    Date = DateTime.Parse("20/02/2019 12:33:16"),
                    Status = 1,
                    TransactionId = "Invoice0000001"
                },
                new EfContext.Transaction
                {
                    Amount = 300M,
                    Currency = "USD",
                    Date = DateTime.Parse("21/02/2019 02:04:59"),
                    Status = 2,
                    TransactionId = "Invoice0000002"
                }
            };

            var fakeValidationResult = new ValidationResult
            {
                IsSucceed = true
            };

            _transactionValidatorMock.Setup(validator =>
            validator.ValidateTransactions(fakeFileParseResult.TransactionList))
                .Returns(fakeValidationResult);

            _unitOfWorkMock.Setup(unit => unit.TransactionRepository).Returns(_transactionRepositoryMock.Object);
            _transactionRepositoryMock.Setup(repo => repo.InsertListAsync(fakeEfContextTransactions));

            _automapperMock.Setup(mapper => mapper.Map<List<EfContext.Transaction>>(fakeFileParseResult.TransactionList))
                .Returns(fakeEfContextTransactions);

            var fileContent = "Invoice0000001, \"1,000.00\", USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                _csvFileParserMock.Setup(parser => parser.ParseFile(stream))
                    .Returns(fakeFileParseResult);

                var parseResult = await _fileManager.ProcessFileAsync(stream, ".csv");
                
                Assert.IsTrue(parseResult.IsSucceed);
                Assert.IsTrue(string.IsNullOrEmpty(parseResult.ErrorMessage));
                Assert.AreEqual(2, parseResult.ProcessedTransactions);
            }
        }
    }
}
