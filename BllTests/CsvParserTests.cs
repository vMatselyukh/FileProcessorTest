using Bll.Helpers;
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

        [SetUp]
        public void Setup()
        {
            _csvOptionsMock = new Mock<IOptions<CsvOptions>>();

            _csvOptionsMock.Setup(csv => csv.Value).Returns(new CsvOptions
            {
                Delimiter = ",",
                SkipHeader = true
            });
        }

        [Test]
        public void TestParseSucceed()
        {
            var fileContent = "Invoice0000001, \"1,0f00.00\", USD, 20/02/2019 12:33:16, Approved\n" +
                                   "Invoice0000002, 300.00, USD, 21/02/2019 02:04:59, Failed";

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                var csvParser = new CsvParser(_csvOptionsMock.Object);
                csvParser.ParseFile(stream);
                Assert.Pass();
            }
        }
    }
}