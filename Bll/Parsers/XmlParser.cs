using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Bll.Parsers
{
    public class XmlParser : IFileParser
    {
        public FileParseResult ParseFile(Stream stream)
        {
            bool isSucceed = true;
            string errorMessage = string.Empty;
            var transactionList = new List<Transaction>();

            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            XDocument docSample = XDocument.Parse(text);



            return new FileParseResult
            {
                IsSucceed = isSucceed,
                ErrorMessage = errorMessage,
                TransactionList = transactionList
            };
        }
    }
}
