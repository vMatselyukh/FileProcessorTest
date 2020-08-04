using AutoMapper;
using Bll.Automapper;
using Bll.Helpers;
using Bll.Parsers;
using Domain.Models.CSV;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;

namespace BllTests
{
    public class CsvParserTests
    {
        private Mock<IOptions<CsvOptions>> _csvOptionsMock;
        private Mock<IServiceProvider> _serviceProviderMock;

        private IMapper _automapper;
        

        [SetUp]
        public void Setup()
        {
            _csvOptionsMock = new Mock<IOptions<CsvOptions>>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _csvOptionsMock.Setup(csv => csv.Value).Returns(new CsvOptions
            {
                Delimiter = ",",
                SkipHeader = false
            });

            var transactionProfile = new TransactionProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(transactionProfile));
            _automapper = new Mapper(configuration);
        }

        [Test]
        public void ParseFile_Succeed()
        {
            var fileContent = "Invoice0000001, \"1,000.00\", USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                var csvParser = new CsvParser(_csvOptionsMock.Object, _automapper, _serviceProviderMock.Object);
                var parseResult = csvParser.ParseFile(stream);

                Assert.IsTrue(parseResult.IsSucceed);
                Assert.IsEmpty(parseResult.ErrorMessage);
                Assert.AreEqual(2, parseResult.TransactionList.Count);
            }
        }

        [Test]
        public void ParseFile_Failed_MissingMember()
        {
            var fileContent = "Invoice0000001, USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            _serviceProviderMock.Setup(provider => provider.GetService(typeof(ErrorMessageHelper)))
                .Returns(new ErrorMessageHelper());

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                var csvParser = new CsvParser(_csvOptionsMock.Object, _automapper, _serviceProviderMock.Object);
                csvParser.ParseFile(stream);
                Assert.Pass();
            }
        }
    }
}