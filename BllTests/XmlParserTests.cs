using AutoMapper;
using Bll.Automapper;
using Bll.Helpers;
using Bll.Parsers;
using Moq;
using NUnit.Framework;
using System;

namespace BllTests
{
    public class XmlParserTests
    {
        private Mock<IServiceProvider> _serviceProviderMock;

        private IMapper _automapper;

        [SetUp]
        public void SetUp()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();

            var transactionProfile = new TransactionProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(transactionProfile));
            _automapper = new Mapper(configuration);
        }

        [Test]
        public void ParseFile_Succeed()
        {
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

            _serviceProviderMock.Setup(provider => provider.GetService(typeof(ErrorMessageHelper)))
                .Returns(new ErrorMessageHelper());

            var xmlParser = new XmlParser(_serviceProviderMock.Object, _automapper);

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                var parseResult = xmlParser.ParseFile(stream);

                Assert.IsTrue(parseResult.IsSucceed);
                Assert.AreEqual(2, parseResult.TransactionList.Count);
            }
        }

        [Test]
        public void ParseFile_Failed_MissingMember()
        {
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
                        "<CurrencyCode></CurrencyCode>" +
                    "</PaymentDetails>" +
                    "<Status>Rejected</Status>" +
                "</Transaction>" +
            "</Transactions>";

            _serviceProviderMock.Setup(provider => provider.GetService(typeof(ErrorMessageHelper)))
                .Returns(new ErrorMessageHelper());

            var xmlParser = new XmlParser(_serviceProviderMock.Object, _automapper);

            using (var stream = StreamHelper.GenerateStreamFromString(fileContent))
            {
                var parseResult = xmlParser.ParseFile(stream);

                Assert.IsFalse(parseResult.IsSucceed);
                Assert.Zero(parseResult.TransactionList.Count);
            }
        }
    }
}
