using Bll.Parsers;
using Domain.Interfaces;
using Domain.Interfaces.Validators;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Bll.Managers
{
    public class FileManager : IFileManager
    {
        private IFileParserFactory _fileParserFactory;
        private ITransactionValidator _transactionValidator;
        private readonly ILogger<FileManager> _logger;
        public FileManager(IFileParserFactory fileParserFactory, ILogger<FileManager> logger,
            ITransactionValidator transactionValidator)
        {
            _fileParserFactory = fileParserFactory;
            _logger = logger;
            _transactionValidator = transactionValidator;
        }
        public FileProcessResult ProcessFile(FileStream fileContent, string fileExtension)
        {
            var parser = _fileParserFactory.GetParser(fileExtension);
            var parseResult = parser.ParseFile(fileContent);

            if (!parseResult.IsSucceed)
            {
                _logger.LogWarning("Errors occured during CSV file parsing. Details: ", parseResult.ErrorMessage);

                return new FileProcessResult
                {
                    IsSucceed = parseResult.IsSucceed,
                    ErrorMessage = parseResult.ErrorMessage
                };
            }

            var validationResult = _transactionValidator.ValidateTransactions(parseResult.TransactionList);

            if (!validationResult.IsSucceed)
            { 
                
            }

            return new FileProcessResult
            {
                IsSucceed = parseResult.IsSucceed,
                ErrorMessage = parseResult.ErrorMessage
            };
        }
    }
}
