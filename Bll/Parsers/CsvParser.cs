using AutoMapper;
using Bll.CSV;
using Bll.Helpers;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.CSV;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace Bll.Parsers
{
    public class CsvParser : IFileParser
    {
        private readonly CsvOptions _csvOptions;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CsvParser> _logger;

        public CsvParser(IOptions<CsvOptions> csvOptions, IMapper mapper,
            IServiceProvider serviceProvider, ILogger<CsvParser> logger)
        {
            _csvOptions = csvOptions.Value;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public FileParseResult ParseFile(Stream stream)
        {
            bool isSucceed = true;
            string errorMessage = string.Empty;
            var transactionList = new List<Transaction>();

            var csvParserOptions = new CsvParserOptions(_csvOptions.SkipHeader, _csvOptions.Delimiter[0]);
            var csvMapper = new CsvTransactionMapping();
            var csvParser = new CsvParser<CsvTransaction>(csvParserOptions, csvMapper);

            var tinyParserResult = csvParser.ReadFromStream(stream, Encoding.UTF8).ToList();

            if (tinyParserResult.Any(row => !row.IsValid))
            {
                isSucceed = false;

                var errorHelper = (ErrorHelper)_serviceProvider.GetService(typeof(ErrorHelper));

                foreach (var failureRow in tinyParserResult.Where(row => !row.IsValid))
                {
                    errorHelper.AppendErrorMessage($"Error in Row: {failureRow.Error.ColumnIndex}. " +
                        $"Unmapped Row: {failureRow.Error.UnmappedRow}. Error Message: {failureRow.Error.Value}.");
                }

                errorMessage = errorHelper.GetErrorMessage();
                _logger.LogWarning("Errors occured during CSV file parsing. Details: ", errorMessage);
            }
            else
            {
                transactionList = _mapper.Map<List<CsvTransaction>, List<Transaction>>(
                    tinyParserResult.Select(row => row.Result).ToList()
                    );
            }

            return new FileParseResult
            {
                IsSucceed = isSucceed,
                ErrorMessage = errorMessage,
                TransactionList = transactionList
            };
        }
    }
}
