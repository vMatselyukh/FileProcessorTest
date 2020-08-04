using AutoMapper;
using Bll.Helpers;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Bll.Parsers
{
    public class XmlParser : IFileParser
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _automapper;
        public XmlParser(IServiceProvider serviceProvider, IMapper automapper)
        {
            _serviceProvider = serviceProvider;
            _automapper = automapper;
        }
        public FileParseResult ParseFile(Stream stream)
        {
            string errorMessage = string.Empty;
            var transactionList = new List<XmlTransaction>();

            XDocument xdoc = XDocument.Load(stream);

            var elementsQuery = from xel in xdoc.Element("Transactions").Elements("Transaction")
                                select new
                                {
                                    Id = xel.Attribute("id")?.Value,
                                    Status = xel.Element("Status")?.Value,
                                    TransactionDate = xel.Element("TransactionDate")?.Value,
                                    PaymentDetails = new
                                    {
                                        Amount = xel.Element("PaymentDetails")?.Element("Amount")?.Value,
                                        CurrencyCode = xel.Element("PaymentDetails")?.Element("CurrencyCode")?.Value,
                                    }
                                };

            var errorMessageHelper = (ErrorMessageHelper)_serviceProvider.GetService(typeof(ErrorMessageHelper));
            bool isSucceed = TryParseXml(elementsQuery, errorMessageHelper, transactionList);


            return new FileParseResult
            {
                IsSucceed = isSucceed,
                ErrorMessage = errorMessageHelper.GetErrorMessage(),
                TransactionList = isSucceed ? _automapper.Map<List<Transaction>>(transactionList) 
                    : new List<Transaction>()
            };
        }

        private bool TryParseXml(IEnumerable<dynamic> elementsQuery, ErrorMessageHelper errorMessageHelper, 
            List<XmlTransaction> transactionList)
        {
            bool isSucceed = true;
            XmlTransactionStatusEnum transactionStatus = default;
            DateTime transactionDate = default;
            decimal transactionAmount = default;
            CurrencyEnum currencyCode = default;

            for (int i = 0; i < elementsQuery.Count(); i++)
            {
                if (elementsQuery.ElementAt(i).Id == null)
                {
                    errorMessageHelper.AppendErrorMessage($"Transaction Id of #{i} transaction is empty.");
                    isSucceed = false;
                }

                if (elementsQuery.ElementAt(i).Status == null
                    || !Enum.TryParse(elementsQuery.ElementAt(i).Status, out transactionStatus))
                {
                    errorMessageHelper.AppendErrorMessage($"Transaction Status of #{i} transaction is empty or has bad format.");
                    isSucceed = false;
                }

                if (elementsQuery.ElementAt(i).TransactionDate == null
                    || !DateTime.TryParse(elementsQuery.ElementAt(i).TransactionDate, out transactionDate))
                {
                    errorMessageHelper.AppendErrorMessage($"Transaction Date of #{i} transaction is empty or has bad format.");
                    isSucceed = false;
                }

                if (elementsQuery.ElementAt(i).PaymentDetails == null)
                {
                    errorMessageHelper.AppendErrorMessage($"Transaction PaymentDetails of #{i} transaction is empty.");
                    isSucceed = false;
                }
                else
                {
                    if (elementsQuery.ElementAt(i).PaymentDetails.Amount == null
                        || !decimal.TryParse(elementsQuery.ElementAt(i).PaymentDetails.Amount, out transactionAmount))
                    {
                        errorMessageHelper.AppendErrorMessage($"Transaction Amount of #{i} transaction is empty or has bad format.");
                        isSucceed = false;
                    }

                    if (elementsQuery.ElementAt(i).PaymentDetails.CurrencyCode == null
                        || !Enum.TryParse(elementsQuery.ElementAt(i).PaymentDetails.CurrencyCode, out currencyCode))
                    {
                        errorMessageHelper.AppendErrorMessage($"Transaction CurrencyCode of #{i} transaction is empty or has bad format.");
                        isSucceed = false;
                    }
                }

                if (isSucceed)
                {
                    transactionList.Add(new XmlTransaction
                    {
                        Amount = transactionAmount,
                        TransactionId = elementsQuery.ElementAt(i).Id,
                        CurrencyCode = currencyCode,
                        TransactionDate = transactionDate,
                        Status = transactionStatus
                    });
                }
            }

            return isSucceed;
        }
    }
}
