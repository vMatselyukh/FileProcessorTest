using Bll.CSV;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.CSV;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace Bll.Parsers
{
    public class CsvParser : IFileParser
    {
        private CsvOptions _csvOptions;
        public CsvParser(IOptions<CsvOptions> csvOptions)
        {
            if (csvOptions == null)
            {
                throw new ArgumentNullException("Csv options can't be null");
            }

            _csvOptions = csvOptions.Value;
        }
        public FileParseResult ParseFile(Stream stream)
        {
            bool isSucceed = true;
            string errorMessage = string.Empty;

            var csvParserOptions = new CsvParserOptions(_csvOptions.SkipHeader, _csvOptions.Delimiter[0]);
            var csvMapper = new CsvTransactionMapping();
            var csvParser = new CsvParser<CsvTransaction>(csvParserOptions, csvMapper);

            var tinyParserResult = csvParser.ReadFromStream(stream, Encoding.UTF8).ToList();

            if (tinyParserResult.Any(row => !row.IsValid))
            {
                isSucceed = false;
                foreach (var failureRow in tinyParserResult.Where(row => !row.IsValid))
                {
                    //log error here
                }

                errorMessage = "message here";
            }
            else
            { 
                //map CSV transaction to transaction
            }

            return new FileParseResult
            {
                IsSucceed = isSucceed,
                ErrorMessage = errorMessage
            };
        }
    }
}
