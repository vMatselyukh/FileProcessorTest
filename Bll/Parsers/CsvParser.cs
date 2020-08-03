using Bll.CSV;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.CSV;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace Bll.Parsers
{
    public class CsvParser : IFileParser
    {
        public FileParseResult ParseFile(Stream stream)
        {
            bool isSucceed = true;
            string errorMessage = string.Empty;

            var csvParserOptions = new CsvParserOptions(false, ',');
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
