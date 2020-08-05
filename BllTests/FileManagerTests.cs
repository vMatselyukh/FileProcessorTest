using AutoMapper;
using Bll.Managers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
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
        private Mock<ITransactionService> _transactionService;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<IMapper> _automapperMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<FileManager>>();
            _fileParserFactoryMock = new Mock<IFileParserFactory>();
            _transactionValidatorMock = new Mock<ITransactionValidator>();
            _transactionService = new Mock<ITransactionService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _automapperMock = new Mock<IMapper>();
            _csvFileParserMock = new Mock<IFileParser>();

            _fileManager = new FileManager(_fileParserFactoryMock.Object, _loggerMock.Object,
                _transactionValidatorMock.Object, _transactionService.Object, _automapperMock.Object);
        }

        [Test]
        public async Task ProcessCsvFile_Succeed()
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

            _transactionRepositoryMock.Setup(repo => repo.InsertListAsync(fakeEfContextTransactions));

            _automapperMock.Setup(mapper => mapper.Map<List<EfContext.Transaction>>(fakeFileParseResult.TransactionList))
                .Returns(fakeEfContextTransactions);

            var fileContent = "Invoice0000001, \"1,000.00\", USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            
            _csvFileParserMock.Setup(parser => parser.ParseFile(fileContent))
                .Returns(fakeFileParseResult);

            var parseResult = await _fileManager.ProcessFileAsync(fileContent, ".csv");
                
            Assert.IsTrue(parseResult.IsSucceed);
            Assert.IsTrue(string.IsNullOrEmpty(parseResult.ErrorMessage));
            Assert.AreEqual(2, parseResult.ProcessedTransactions);
        }

        [Test]
        public async Task ProcessXmlFile_Succeed()
        {
            _fileParserFactoryMock.Setup(factory => factory.GetParser(".xml"))
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

            _transactionRepositoryMock.Setup(repo => repo.InsertListAsync(fakeEfContextTransactions));

            _automapperMock.Setup(mapper => mapper.Map<List<EfContext.Transaction>>(fakeFileParseResult.TransactionList))
                .Returns(fakeEfContextTransactions);

            var fileContent = $"<Transactions>" +
                "<Transaction id=\"Inv00001\">" +
                    "<TransactionDate>2019-01-23T13:45:10</TransactionDate>" +
                    "<PaymentDetails>" +
                        "<Amount>200.00</Amount>" +
                        "<CurrencyCode>USD</CurrencyCode>" +
                    "</PaymentDetails>" +
                    "<Status>Done</Status>" +
                "</Transaction>" +
                "<Transaction id=\"Inv00002\">" +
                    "<TransactionDate>2019-01-24T16:09:15</TransactionDate>" +
                    "<PaymentDetails>" +
                        "<Amount>10000.00</Amount>" +
                        "<CurrencyCode>EUR</CurrencyCode>" +
                    "</PaymentDetails>" +
                    "<Status>Rejected</Status>" +
                "</Transaction>" +
            "</Transactions>";

            _csvFileParserMock.Setup(parser => parser.ParseFile(fileContent))
                .Returns(fakeFileParseResult);

            var parseResult = await _fileManager.ProcessFileAsync(fileContent, ".xml");

            Assert.IsTrue(parseResult.IsSucceed);
            Assert.IsTrue(string.IsNullOrEmpty(parseResult.ErrorMessage));
            Assert.AreEqual(2, parseResult.ProcessedTransactions);
        }
    }
}
