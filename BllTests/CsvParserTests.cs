using AutoMapper;
using Bll.Automapper;
using Bll.Parsers;
using Domain.Models.CSV;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace BllTests
{
    public class CsvParserTests
    {
        private Mock<IOptions<CsvOptions>> _csvOptionsMock;

        private IMapper _automapper;
        

        [SetUp]
        public void Setup()
        {
            _csvOptionsMock = new Mock<IOptions<CsvOptions>>();

            _csvOptionsMock.Setup(csv => csv.Value).Returns(new CsvOptions
            {
                Delimiter = ",",
                SkipHeader = false,
                NewLineSeparators = new[] { "\n" }
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

            var csvParser = new CsvParser(_csvOptionsMock.Object, _automapper);
            var parseResult = csvParser.ParseFile(fileContent);

            Assert.IsTrue(parseResult.IsSucceed);
            Assert.IsEmpty(parseResult.ErrorMessage);
            Assert.AreEqual(2, parseResult.TransactionList.Count);
        }

        [Test]
        public void ParseFile_Failed_MissingMember()
        {
            var fileContent = "Invoice0000001, USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            var csvParser = new CsvParser(_csvOptionsMock.Object, _automapper);
            csvParser.ParseFile(fileContent);
            Assert.Pass();
        }
    }
}