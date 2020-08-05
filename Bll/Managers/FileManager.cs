using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.Managers
{
    public class FileManager : IFileManager
    {
        private readonly IFileParserFactory _fileParserFactory;
        private readonly ITransactionValidator _transactionValidator;
        private readonly ILogger<FileManager> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _automapper;
        public FileManager(IFileParserFactory fileParserFactory, ILogger<FileManager> logger,
            ITransactionValidator transactionValidator, ITransactionService transactionService, IMapper automapper)
        {
            _fileParserFactory = fileParserFactory;
            _logger = logger;
            _transactionValidator = transactionValidator;
            _transactionService = transactionService;
            _automapper = automapper;
        }
        public async Task<FileProcessResult> ProcessFileAsync(string content, string fileExtension)
        {
            var parser = _fileParserFactory.GetParser(fileExtension);
            var parseResult = parser.ParseFile(content);

            if (!parseResult.IsSucceed)
            {
                _logger.LogWarning("Errors occured during CSV file parsing. Details: ", parseResult.ErrorMessage);

                return new FileProcessResult
                {
                    IsSucceed = false,
                    ErrorMessage = parseResult.ErrorMessage
                };
            }

            var validationResult = _transactionValidator.ValidateTransactions(parseResult.TransactionList);

            if (!validationResult.IsSucceed)
            {
                return new FileProcessResult
                {
                    IsSucceed = false,
                    ErrorMessage = validationResult.ErrorMessage
                };
            }

            await _transactionService.InsertListAsync(
                _automapper.Map<List<EfContext.Transaction>>(parseResult.TransactionList));

            return new FileProcessResult
            {
                IsSucceed = true,
                ProcessedTransactions = parseResult.TransactionList.Count
            };
        }
    }
}
