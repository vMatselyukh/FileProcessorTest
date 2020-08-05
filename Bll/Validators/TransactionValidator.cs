using Bll.Helpers;
using Domain.Interfaces;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Models.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Bll.Validators
{
    public class TransactionValidator : ITransactionValidator
    {
        private ValidationRules _validationRules;
        private IMessageBuilder _errorMessageHelper;
        private IServiceProvider _serviceProvider;

        public TransactionValidator(IOptions<ValidationRules> validationRules, IMessageBuilder errorMessageHelper,
            IServiceProvider serviceProvider)
        {
            _validationRules = validationRules.Value;
            _errorMessageHelper = errorMessageHelper;
            _serviceProvider = serviceProvider;
        }

        public ValidationResult ValidateTransaction(Transaction transaction)
        {
            if (transaction.TransactionId.Length > _validationRules.TransactionNameMaxLength)
            {
                return new ValidationResult
                {
                    IsSucceed = false,
                    ErrorMessage = $"Transaction id {transaction.TransactionId} is too long. " +
                    $"Max length is {_validationRules.TransactionNameMaxLength}."
                };
            }

            return new ValidationResult
            {
                IsSucceed = true
            };
        }

        public ValidationResult ValidateTransactions(List<Transaction> transactions)
        {
            var messageBuilder = (MessageBuilder)_serviceProvider.GetService(typeof(MessageBuilder));

            var isSucceed = true;

            foreach (var transaction in transactions)
            {
                var result = ValidateTransaction(transaction);
                if (!result.IsSucceed)
                {
                    messageBuilder.AppendMessage(result.ErrorMessage);
                    isSucceed = false;
                }
            }

            return new ValidationResult
            {
                ErrorMessage = messageBuilder.GetMessage(),
                IsSucceed = isSucceed
            };
        }
    }
}
